
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
        StartCoroutine(BuffCoroutine(caster, target, buffData));
    }

    private IEnumerator BuffCoroutine(Fighter caster, Fighter target, BuffData buffData) {

        List<GameObject> particles = new List<GameObject>();
        
        // Apply Immediate Buff
        foreach (BuffMiniData data in buffData.ImmediateEffectBuff) {
            ApplyBuff(caster, target, data, particles);
        }
        
        // Apply Long Time Buff
        WaitForSeconds wait = new WaitForSeconds(buffData.TickInterval);
        for (float t = 0.0f; t < buffData.Duration; t += buffData.TickInterval) {
            foreach (BuffMiniData data in buffData.LongTimeEffectBuff) {
                ApplyBuff(caster, target, data, particles);
            }
            yield return wait;
        }
        
        // Remove Immediate Buff
        foreach (BuffMiniData data in buffData.ImmediateEffectBuff) {
            RemoveImmediateBuff(caster, target, data);
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

    private void RemoveImmediateBuff(Fighter caster, Fighter target, BuffMiniData data) {
        Fighter refFighter = data.Ref == BuffRef.Caster ? caster : target;
        FighterProperty refProperty = data.Ref == BuffRef.Caster ? data.CasterProperty : data.TargetRefProperty;
        target.FighterPropertyChange(data.TargetUpdateProperty, refProperty, data.ModifyWay, data.ChangedValue, false, refFighter);
    }

    private void ApplyBuff(Fighter caster, Fighter target, BuffMiniData data, List<GameObject> particles) {
        if (data.EffectParticlePrefab) {
            GameObject particle = Instantiate(data.EffectParticlePrefab, target.transform);
            if (data.IsDestroyImmediate) {
                Destroy(particle, 1.0f);
            } else {
                particles.Add(particle);   
            }
        }
        
        Fighter refFighter = data.Ref == BuffRef.Caster ? caster : target;
        FighterProperty refProperty = data.Ref == BuffRef.Caster ? data.CasterProperty : data.TargetRefProperty;
        if (data.TargetUpdateProperty != FighterProperty.Health || data.IsChangeProperty) {
            target.FighterPropertyChange(data.TargetUpdateProperty, refProperty, data.ModifyWay, data.ChangedValue, true, refFighter);
            return;
        }
        
        float value = target.GetPropertyChangeValue(refProperty, data.ModifyWay, data.ChangedValue, true, refFighter);
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

