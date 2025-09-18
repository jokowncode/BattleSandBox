
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (!target || target.IsDead) return;
        if (!caster || caster.IsDead) return;
        Coroutine coroutine = StartCoroutine(BuffCoroutine(caster, target, buffData));
        target.OnDead += _ => StopCoroutine(coroutine);
    }

    private IEnumerator BuffCoroutine(Fighter caster, Fighter target, BuffData buffData) {
        
        Dictionary<FighterProperty, float> buffChangeProperty = new Dictionary<FighterProperty, float>();

        if (buffData.ParticlePrefab) {
            GameObject particle = Instantiate(buffData.ParticlePrefab, target.Center.transform);
            particle.transform.localPosition = Vector3.zero;
        }

        // Apply Immediate Buff
        foreach (BuffMiniData data in buffData.ImmediateEffectBuff) {
            ApplyBuff(caster, target, data, true, buffChangeProperty);
        }
        
        // Apply Long Time Buff
        Dictionary<BuffMiniData, float> longTimeBuffChangeValue = new Dictionary<BuffMiniData, float>();
        WaitForSeconds wait = new WaitForSeconds(buffData.TickInterval);
        for (float t = 0.0f; buffData.Duration < 0.0f || t < buffData.Duration; t += buffData.TickInterval) {
            if (target.IsDisappear) {
                yield return wait;
                continue;
            }
            bool isFirstTime = t == 0.0f;
            foreach (BuffMiniData data in buffData.LongTimeEffectBuff) {
                if (isFirstTime) {
                    float changeValue = ApplyBuff(caster, target, data, true, buffChangeProperty);
                    longTimeBuffChangeValue.Add(data, changeValue);
                } else {
                    ApplyBuff(data, target, longTimeBuffChangeValue[data], false, buffChangeProperty);
                }
            }
            yield return wait;
        }
        
        // Remove Property Change
        foreach (KeyValuePair<FighterProperty, float> pair in buffChangeProperty) {
            target.FighterPropertyChange(pair.Key, pair.Key, PropertyModifyWay.Value, PropertyRef.Initial, pair.Value, false);
        }
        
        // Remove Particle
        foreach (Transform buffParticle in target.Center) {
            if(buffParticle) Destroy(buffParticle.gameObject);    
        }
        
        // Apply Last Buff
        foreach (BuffData data in buffData.LastEffectBuff) {
            AddBuff(caster, target, data);
        }
    }

    private void ApplyParticle(BuffMiniData data, Fighter target, bool isFirstTime) {
        if (!data.EffectParticlePrefab) return;
        if (!data.IsDestroyImmediate && !isFirstTime) return;
        GameObject particle = Instantiate(data.EffectParticlePrefab, target.Center.transform);
        particle.transform.localPosition = Vector3.zero;
        if (data.IsDestroyImmediate) {
            Destroy(particle, data.DestroyDelay);    
        }
    }

    private void ApplyBuff(BuffMiniData data, Fighter target, float changeValue, bool isFirstTime, 
        Dictionary<FighterProperty, float> record) {
        ApplyParticle(data, target, isFirstTime);
        
        if (data.TargetUpdateProperty != FighterProperty.Health || data.IsChangeProperty) {
            target.FighterPropertyChange(data.TargetUpdateProperty, data.TargetUpdateProperty, 
                PropertyModifyWay.Value, data.PropertyRef, changeValue, true);
            if (!record.TryAdd(data.TargetUpdateProperty, changeValue)) {
                record[data.TargetUpdateProperty] += changeValue;
            }
            return;
        }
        
        HealOrDamageTarget(target, data.EffectParticlePrefab, changeValue);
    }

    private float ApplyBuff(Fighter caster, Fighter target, BuffMiniData data, bool isFirstTime,
         Dictionary<FighterProperty, float> record) {
        
        ApplyParticle(data, target, isFirstTime);
        
        Fighter refFighter = data.Ref == BuffRef.Caster ? caster : target;
        FighterProperty refProperty = data.Ref == BuffRef.Caster ? data.CasterProperty : data.TargetRefProperty;
        if (data.TargetUpdateProperty != FighterProperty.Health || data.IsChangeProperty) {
            float changeValue = target.FighterPropertyChange(data.TargetUpdateProperty, refProperty, data.ModifyWay, data.PropertyRef, data.ChangedValue, true, refFighter);
            if (!record.TryAdd(data.TargetUpdateProperty, changeValue)) {
                record[data.TargetUpdateProperty] += changeValue;
            }
            return changeValue;
        }
        
        float value = target.GetPropertyChangeValue(refProperty, data.ModifyWay, data.PropertyRef, data.ChangedValue, true, refFighter);
        HealOrDamageTarget(target, data.EffectParticlePrefab, value);
        return value;
    }

    private void HealOrDamageTarget(Fighter target, bool hasParticle, float value) {
        EffectData effect = new EffectData {
            Value = Mathf.Abs(value),
            IsCritical = false,
            NotShowParticle = hasParticle
        };
        if (value > 0.0f) {
            target.BeHealed(effect);
        } else if(value < 0.0f) {
            target.BeDamaged(effect);
        }
    }
}

