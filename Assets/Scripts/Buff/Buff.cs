using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Buff : MonoBehaviour
{

    public GameObject immediateEffectPrefab;  // 立即效果粒子预制体
    public GameObject tickEffectPrefab;       // 持续效果粒子预制体
    //public float TimeRemaining { get; private set; }

    private float changedShieldValue;
    private float changedMoveSpeedValue;
    private float changedAttackSpeedValue;
    private float changedPhysicsAttackValue;
    private float changedMagicAttackValue;
    private float changedCriticalValue;
    
    private List<GameObject> spawnedParticles = new List<GameObject>(); // 记录所有粒子对象
    
    public void AddBuff(Fighter caster,Fighter target,BuffData buffData)
    {
        Debug.Log("AddBuff");
        StartCoroutine(BuffRoutine(caster,target,buffData));
    }
    
    private IEnumerator BuffRoutine(Fighter caster,Fighter target,BuffData buffData)
    {
        //spawnedParticles.Clear();
        List<GameObject> routineSpawnedParticles = new List<GameObject>();
        int routineId = GetInstanceID(); // 或自己生成唯一 ID
        
        changedShieldValue = 0f;
        changedMoveSpeedValue = 0f;
        changedAttackSpeedValue = 0f;
        changedPhysicsAttackValue = 0f;
        changedMagicAttackValue = 0f;
        
        float TimeRemaining = buffData.duration;
        
        foreach (var buffMiniData in buffData.immediateEffectBuff)
        {
            // Debug.Log("AddBuffMiniData"+buffMiniData);
            CalculateChangedValue(caster,target,buffMiniData);
        }

        ApplyImmediateBuffEffects(target, buffData);
        target.UpdateShieldAmount();
        target.UpdateBloodAmount();

        // GameObject effect = null;
        // 立即生成一次粒子特效
        if (immediateEffectPrefab != null)
        {
            GameObject effect = Instantiate(immediateEffectPrefab, target.Center.position, Quaternion.identity);
            //Debug.Log("bbb" + effect.name);
            routineSpawnedParticles.Add(effect);
            //Debug.Log("wow");
            spawnedParticles.Add(effect);
        }
        
        if (buffData.tickInterval > 0)
        {
            // 间隔触发效果
            WaitForSeconds wait = new WaitForSeconds(buffData.tickInterval);
            
            while (TimeRemaining > 0)
            {
                yield return wait;
                TimeRemaining -= buffData.tickInterval;
                
                ApplyLongTimeBuffEffects(caster,target,buffData);
                target.UpdateShieldAmount();
                target.UpdateBloodAmount();
                // Debug.Log("Dot");
                // 每次 tick 生成一次粒子
                if (tickEffectPrefab != null&&target!=null)
                {
                    GameObject effect = Instantiate(tickEffectPrefab, target.Center.position, Quaternion.identity);
                    Debug.Log("ccc" + effect.name);
                    routineSpawnedParticles.Add(effect);
                    spawnedParticles.Add(effect);
                    Debug.Log("携程 "+routineId+" 持续伤害开始");
                }
            }
        }
        else
        {
            // 单次效果，等待持续时间结束
            yield return new WaitForSeconds(buffData.duration);
            TimeRemaining = 0;
        }

        // if (BuffDetail.isRemoveBuff)
        // {
        RemoveBuff(target);
        ApplyLastBuffEffects(target, buffData);
        target.UpdateShieldAmount();
        target.UpdateBloodAmount();
        //Debug.Log("Removed buff");
        // }
        //yield return null;
        
        // Debug.Log("BuffEnd");
        // 删除所有粒子特效
        Debug.Log(routineSpawnedParticles.Count);
        foreach (GameObject effect in routineSpawnedParticles) {
            Debug.Log(effect.name);
            Destroy(effect);
            spawnedParticles.Remove(effect);
        }
        
        
        /*foreach (var particle in spawnedParticles)
        {
            if (particle != null)
            {
                Destroy(particle);
                spawnedParticles.Remove(particle);
            }
        }*/
        //routineSpawnedParticles.Clear();
        Debug.Log("当前携程 "+ routineId+" 粒子销毁");
    }
    
    private void ApplyLongTimeBuffEffects(Fighter caster =null,Fighter target = null,BuffData buffData = null)
    {
        Debug.Log("ApplyLongTimeBuffEffects");
        foreach (var buffMiniData in buffData.longTimeEffectBuff)
        {
            float value = 0f;
            if (buffMiniData.basicRef == BasicRef.Caster)
            {
                if(buffMiniData.refProperty==FighterProperty.MagicAttack||buffMiniData.refProperty==FighterProperty.PhysicsAttack)
                {
                    if(caster.Type == FighterType.Warrior)
                        value = caster.GetPropertyData(FighterProperty.PhysicsAttack) * buffMiniData.changedValue;
                    else
                        value = caster.GetPropertyData(FighterProperty.MagicAttack) * buffMiniData.changedValue;
                }
                else
                    value = caster.GetPropertyData(buffMiniData.refProperty) * buffMiniData.changedValue;
            }
            else if (buffMiniData.basicRef == BasicRef.Target)
            {
                if(buffMiniData.refProperty==FighterProperty.MagicAttack||buffMiniData.refProperty==FighterProperty.PhysicsAttack)
                {
                    if(target.Type == FighterType.Warrior)
                        value = target.GetPropertyData(FighterProperty.PhysicsAttack) * buffMiniData.changedValue;
                    else
                        value = target.GetPropertyData(FighterProperty.MagicAttack) * buffMiniData.changedValue;
                }
                else
                    value = target.GetPropertyData(buffMiniData.refProperty) * buffMiniData.changedValue;
            }
            else
            {
                value = buffMiniData.changedValue;
            }
            // Debug.Log("LongTimeBuff: "+value);
            target.FighterPropertyChange(buffMiniData.changedProperty, PropertyModifyWay.Value, value, true);
        }
    }
    
    private void ApplyImmediateBuffEffects(Fighter target = null,BuffData buffData = null)
    {
        // 立即应用一次效果
        //ApplyBuffEffects(target);
        Debug.Log("ApplyImmediateBuffEffects: "+changedShieldValue);
        if (changedShieldValue != 0)
        {
            target.InitialShield = changedShieldValue;
            target.Shield = changedShieldValue;
            target.UpdateShieldAmount();
        }
        if(changedAttackSpeedValue!=0)
            target.FighterPropertyChange(FighterProperty.CooldownPercentage,PropertyModifyWay.Value,changedAttackSpeedValue,true);
        if(changedMoveSpeedValue != 0)
            target.FighterPropertyChange(FighterProperty.Speed,PropertyModifyWay.Value,-changedMoveSpeedValue,true);
        if(changedPhysicsAttackValue !=0)
            target.FighterPropertyChange(FighterProperty.PhysicsAttack,PropertyModifyWay.Value,changedPhysicsAttackValue,true);
        if(changedMagicAttackValue !=0)
            target.FighterPropertyChange(FighterProperty.MagicAttack,PropertyModifyWay.Value,changedMagicAttackValue,true);
        if(changedCriticalValue !=0)
            target.FighterPropertyChange(FighterProperty.Critical,PropertyModifyWay.Value,changedCriticalValue,true);
    }
    
    private void ApplyLastBuffEffects(Fighter target = null,BuffData buffData = null)
    {
        foreach (var buffMiniData in buffData.lastEffectBuff)
        {
            float value = 0f;
            if (buffMiniData.basicRef == BasicRef.Target)
            {
                value = target.GetPropertyData(buffMiniData.changedProperty) * buffMiniData.changedValue;
            }
            else
            {
                value = buffMiniData.changedValue;
            }
            //Debug.Log("LastBuff: "+value);
            target.FighterPropertyChange(buffMiniData.changedProperty, PropertyModifyWay.Value, value, true);
        }
    }

    public void CalculateChangedValue(Fighter caster,Fighter target,BuffMiniData buffMiniData)
    {
        
        float value;
        //Debug.Log(buffMiniData.basicRef);
        if (buffMiniData.basicRef == BasicRef.Caster)
        {
            if(buffMiniData.changedProperty == FighterProperty.Shield)
                value = caster.GetPropertyData(FighterProperty.Health) * buffMiniData.changedValue;
            else
                value = caster.GetPropertyData(buffMiniData.changedProperty) * buffMiniData.changedValue;
        }
        else if (buffMiniData.basicRef == BasicRef.Target)
        {
            if(buffMiniData.changedProperty == FighterProperty.Shield)
                value = target.GetPropertyData(FighterProperty.Health) * buffMiniData.changedValue;
            else
                value = target.GetPropertyData(buffMiniData.changedProperty) * buffMiniData.changedValue;
        }
        else
        {
            value = buffMiniData.changedValue;
        }
        // Debug.Log("CalculateValue"+value);
        //target.HeroPropertyChange(FighterProperty.CooldownPercentage,PropertyModifyWay.Percentage,speed,true);
        if (buffMiniData.changedProperty == FighterProperty.Shield)
            changedShieldValue += value;
        if (buffMiniData.changedProperty == FighterProperty.CooldownPercentage)
            changedAttackSpeedValue += value;
        if (buffMiniData.changedProperty == FighterProperty.Speed)
            changedMoveSpeedValue += 1;
        if (buffMiniData.changedProperty == FighterProperty.PhysicsAttack)
            changedPhysicsAttackValue += value;
        if (buffMiniData.changedProperty == FighterProperty.MagicAttack)
            changedMagicAttackValue += value;
        if (buffMiniData.changedProperty == FighterProperty.Critical)
            changedCriticalValue += value;
    }

    public void AddAttackSpeed(Hero tf, float speed)
    {
        tf.FighterPropertyChange(FighterProperty.CooldownPercentage,PropertyModifyWay.Percentage,speed,true);
    }

    public void RemoveBuff(Fighter target)
    {
        if (changedShieldValue != 0)
            target.Shield = 0;
        if(changedAttackSpeedValue!=0)
            target.FighterPropertyChange(FighterProperty.CooldownPercentage,PropertyModifyWay.Value,changedAttackSpeedValue,false);
        if(changedMoveSpeedValue != 0)
            target.FighterPropertyChange(FighterProperty.Speed,PropertyModifyWay.Value,changedMoveSpeedValue,false);
        if(changedPhysicsAttackValue !=0)
            target.FighterPropertyChange(FighterProperty.PhysicsAttack,PropertyModifyWay.Value,changedPhysicsAttackValue,false);
        if(changedMagicAttackValue !=0)
            target.FighterPropertyChange(FighterProperty.MagicAttack,PropertyModifyWay.Value,changedMagicAttackValue,false);
        if(changedCriticalValue !=0)
            target.FighterPropertyChange(FighterProperty.Critical,PropertyModifyWay.Value,changedCriticalValue,false);
    }

    private void OnDestroy()
    {
        foreach (var particle in spawnedParticles)
        {
            if (particle != null)
            {
                //if(particle.layer)
                Destroy(particle);
            }
        }
        Debug.Log("消除所有粒子");
        spawnedParticles.Clear();
    }
    
    public void DestroyShieldParticles()
    {
        // 用临时列表记录要移除的粒子
        List<GameObject> toRemove = new List<GameObject>();

        foreach (var particle in spawnedParticles)
        {
            if (particle != null && particle.CompareTag("Shield"))
            {
                Destroy(particle);
                toRemove.Add(particle);
            }
        }

        // 从spawnedParticles移除这些粒子
        foreach (var particle in toRemove)
        {
            spawnedParticles.Remove(particle);
        }

        Debug.Log("已销毁所有 Shield 粒子");
    }
    
    
}
