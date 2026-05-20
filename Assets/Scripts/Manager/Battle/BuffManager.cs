
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffManager : MonoBehaviour {

    public static BuffManager Instance;

    private readonly Dictionary<Fighter, Dictionary<CascadeBuffType, int>> FighterBuffs = new();
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void AddBuff(Fighter caster, Fighter target, BuffData buffData, int count = 1) {
        if (!target || target.IsDead) return;
        if (!caster || caster.IsDead) return;

        for (int i = 0; i < count; i++) {
            if (buffData.CascadeType != CascadeBuffType.None && buffData.LimitCount > 0)  {
                if (TryGetFighterBuffCount(target, buffData.CascadeType, out int curCount) && curCount >= buffData.LimitCount) {
                    break;
                }
                
                if (!FighterBuffs.ContainsKey(target)) {
                    FighterBuffs.Add(target, new Dictionary<CascadeBuffType, int>());
                }

                if (!FighterBuffs[target].TryAdd(buffData.CascadeType, 1)) {
                    FighterBuffs[target][buffData.CascadeType] += 1;
                }
            }
            Coroutine coroutine = StartCoroutine(BuffCoroutine(caster, target, buffData));
            target.OnDead += _ => StopCoroutine(coroutine);
        }
    }

    private IEnumerator BuffCoroutine(Fighter caster, Fighter target, BuffData buffData) {
        
        Dictionary<FighterProperty, float> buffChangeProperty = new Dictionary<FighterProperty, float>();
        List<PoolGO> buffParticles = new List<PoolGO>();

        if (buffData.ParticlePrefab) {
            // GameObject particle = Instantiate(buffData.ParticlePrefab, target.Center.transform);
            // particle.transform.localPosition = Vector3.zero;
            
            Transform parentTrans = buffData.ParticleGenerateCenter ? target.Center.transform : target.transform;
            PoolGO particle = PoolManager.Instance.GetGameObject(buffData.ParticlePrefab, parentTrans);
            buffParticles.Add(particle);
        }

        // Apply Immediate Buff
        foreach (BuffMiniData data in buffData.ImmediateEffectBuff) {
            ApplyBuff(caster, target, data, true, buffChangeProperty, buffParticles);
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
                    float changeValue = ApplyBuff(caster, target, data, true, buffChangeProperty, buffParticles);
                    longTimeBuffChangeValue.Add(data, changeValue);
                } else {
                    ApplyBuff(data, target, longTimeBuffChangeValue[data], false, buffChangeProperty, buffParticles);
                }
            }
            yield return wait;
        }
        
        // Remove Property Change
        if (target) {
            foreach (KeyValuePair<FighterProperty, float> pair in buffChangeProperty) {
                target.FighterPropertyChange(pair.Key, pair.Key, PropertyModifyWay.Value, PropertyRef.Initial, pair.Value, false);
            }    
        }
        
        // Remove Particle
        foreach (PoolGO buffParticle in buffParticles) {
            // if(buffParticle) Destroy(buffParticle);    
            PoolManager.Instance.ReleaseGameObject(buffParticle);
        }
        
        // Apply Last Buff
        foreach (BuffData data in buffData.LastEffectBuff) {
            AddBuff(caster, target, data);
        }

        if (target && HasFighterBuff(target, buffData.CascadeType)) {
            FighterBuffs[target][buffData.CascadeType] -= 1;
            if (FighterBuffs[target][buffData.CascadeType] <= 0) {
                FighterBuffs[target].Remove(buffData.CascadeType);
            }
        }
    }

    private void ApplyParticle(BuffMiniData data, Fighter target, bool isFirstTime, List<PoolGO> particles) {
        if (!data.EffectParticlePrefab) return;
        if (!data.IsDestroyImmediate && !isFirstTime) return;
        
        // GameObject particle = Instantiate(data.EffectParticlePrefab, target.Center.transform);
        // particle.transform.localPosition = Vector3.zero;

        Transform parentTrans = data.EffectParticleGenerateCenter ? target.Center.transform : target.transform;
        PoolGO particle = PoolManager.Instance.GetGameObject(data.EffectParticlePrefab, parentTrans);
        
        if (data.IsDestroyImmediate) {
            // Destroy(particle, data.DestroyDelay);    
            PoolManager.Instance.ReleaseGameObject(particle, data.DestroyDelay);
        } else {
            particles.Add(particle);
        }
    }

    private void ApplyBuff(BuffMiniData data, Fighter target, float changeValue, bool isFirstTime, 
        Dictionary<FighterProperty, float> record, List<PoolGO> particles) {
        ApplyParticle(data, target, isFirstTime, particles);
        
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
         Dictionary<FighterProperty, float> record, List<PoolGO> particles) {
        
        ApplyParticle(data, target, isFirstTime, particles);
        
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

    private bool HasFighterBuff(Fighter fighter, CascadeBuffType type) {
        return this.FighterBuffs.ContainsKey(fighter) && this.FighterBuffs[fighter].ContainsKey(type);
    }

    public bool TryGetFighterBuffCount(Fighter fighter, CascadeBuffType type, out int count) {
        bool result = HasFighterBuff(fighter, type);
        count = result ? this.FighterBuffs[fighter][type] : -1;
        return result;
    }
}

