
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour {

    public static BuffManager Instance;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void AddBuff(Fighter caster, Fighter target, BuffData buffData) {
        Coroutine coroutine = StartCoroutine(BuffCoroutine(caster, target, buffData));
        target.OnDead += () => StopCoroutine(coroutine);
    }

    private IEnumerator BuffCoroutine(Fighter caster, Fighter target, BuffData buffData) {
        
        List<GameObject> particles = new List<GameObject>();
        Dictionary<FighterProperty, float> immediateBuffChangeValue = new Dictionary<FighterProperty, float>();
        
        // Apply Immediate Buff
        foreach (BuffMiniData data in buffData.ImmediateEffectBuff) {
            ApplyBuff(caster, target, data, true, particles, immediateBuffChangeValue);
        }
        
        // Apply Long Time Buff
        WaitForSeconds wait = new WaitForSeconds(buffData.TickInterval);
        for (float t = 0.0f; buffData.Duration < 0.0f || t < buffData.Duration; t += buffData.TickInterval) {
            foreach (BuffMiniData data in buffData.LongTimeEffectBuff) {
                bool isFirstTime = t == 0.0f;
                ApplyBuff(caster, target, data, isFirstTime, particles, immediateBuffChangeValue);
            }
            yield return wait;
        }
        
        // Remove Property Change
        foreach (KeyValuePair<FighterProperty, float> pair in immediateBuffChangeValue) {
            target.FighterPropertyChange(pair.Key, pair.Key, PropertyModifyWay.Value, PropertyRef.Initial, pair.Value, false);
        }
        
        // Remove Particle
        foreach (GameObject particle in particles) {
            if(particle) Destroy(particle);    
        }
        
        // Apply Last Buff
        foreach (BuffData data in buffData.LastEffectBuff) {
            AddBuff(caster, target, data);
        }
    }

    private void ApplyBuff(Fighter caster, Fighter target, BuffMiniData data, bool isFirstTime,
        List<GameObject> particles, Dictionary<FighterProperty, float> record) {
        if (data.EffectParticlePrefab) {
            if (data.IsDestroyImmediate || isFirstTime) {
                GameObject particle = Instantiate(data.EffectParticlePrefab, target.transform);
                if (data.IsDestroyImmediate) {
                    Destroy(particle, data.DestroyDelay);    
                } else {
                    particles.Add(particle);    
                }
            }
        }
        
        Fighter refFighter = data.Ref == BuffRef.Caster ? caster : target;
        FighterProperty refProperty = data.Ref == BuffRef.Caster ? data.CasterProperty : data.TargetRefProperty;
        if (data.TargetUpdateProperty != FighterProperty.Health || data.IsChangeProperty) {
            float changeValue = target.FighterPropertyChange(data.TargetUpdateProperty, refProperty, data.ModifyWay, data.PropertyRef, data.ChangedValue, true, refFighter);
            if (!record.TryAdd(data.TargetUpdateProperty, changeValue)) {
                record[data.TargetUpdateProperty] += changeValue;
            }
            return;
        }
        
        float value = target.GetPropertyChangeValue(refProperty, data.ModifyWay, data.PropertyRef, data.ChangedValue, true, refFighter);
        EffectData effect = new EffectData {
            Value = Mathf.Abs(value),
            IsCritical = false
        };
        if (value > 0.0f) {
            target.BeHealed(effect);
        } else if(value < 0.0f) {
            target.BeDamaged(effect);
        }
    }
}

