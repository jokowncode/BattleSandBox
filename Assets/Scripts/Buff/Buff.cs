using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    // public static Buff Instance;
    //
    // private void Awake() {
    //     if (Instance != null) {
    //         Destroy(this.gameObject);
    //         return;
    //     }
    //     Instance = this;
    //     DontDestroyOnLoad(this.gameObject);
    // }

    //public BuffData BuffDetail;
    //public FighterData EffectProp;
    //public Fighter Target { get; }
    public float TimeRemaining { get; private set; }

    private float changedShieldValue;
    private float changedMoveSpeedValue;
    private float changedAttackSpeedValue;
    private float changedPhysicsAttackValue;
    private float changedMagicAttackValue;
    private float changedCriticalValue;
    
    public void AddBuff(Fighter caster,Fighter target,BuffData buffData)
    {
        Debug.Log("AddBuff");
        StartCoroutine(BuffRoutine(caster,target,buffData));
    }
    
    private IEnumerator BuffRoutine(Fighter caster,Fighter target,BuffData buffData)
    {
        
        changedShieldValue = 0f;
        changedMoveSpeedValue = 0f;
        changedAttackSpeedValue = 0f;
        changedPhysicsAttackValue = 0f;
        changedMagicAttackValue = 0f;
        
        TimeRemaining = buffData.duration;
        
        foreach (var buffMiniData in buffData.immediateEffectBuff)
        {
            Debug.Log("AddBuffMiniData"+buffMiniData);
            CalculateChangedValue(caster,target,buffMiniData);
        }

        ApplyImmediateBuffEffects(target, buffData);
        
        if (buffData.tickInterval > 0)
        {
            // 间隔触发效果
            WaitForSeconds wait = new WaitForSeconds(buffData.tickInterval);
            
            while (TimeRemaining > 0)
            {
                yield return wait;
                TimeRemaining -= buffData.tickInterval;
                
                ApplyLongTimeBuffEffects(target,buffData);
                Debug.Log("Dot");
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
        //Debug.Log("Removed buff");
        // }
        //yield return null;
        
        Debug.Log("BuffEnd");
    }
    
    private void ApplyLongTimeBuffEffects(Fighter target = null,BuffData buffData = null)
    {
        foreach (var buffMiniData in buffData.longTimeEffectBuff)
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
            // Debug.Log("LongTimeBuff: "+value);
            target.FighterPropertyChange(buffMiniData.changedProperty, PropertyModifyWay.Value, value, true);
        }
    }
    
    private void ApplyImmediateBuffEffects(Fighter target = null,BuffData buffData = null)
    {
        // 立即应用一次效果
        //ApplyBuffEffects(target);
        Debug.Log("ApplyImmediateBuffEffects: "+changedAttackSpeedValue);
        if (changedShieldValue != 0)
        {
            target.Shield = changedShieldValue;
            //target.UpdateShieldAmount();
        }
        if(changedAttackSpeedValue!=0)
            target.FighterPropertyChange(FighterProperty.CooldownPercentage,PropertyModifyWay.Value,changedAttackSpeedValue,true);
        if(changedMoveSpeedValue != 0)
            target.FighterPropertyChange(FighterProperty.Speed,PropertyModifyWay.Value,changedMoveSpeedValue,true);
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
        Debug.Log("CalculateValue"+value);
        //target.HeroPropertyChange(FighterProperty.CooldownPercentage,PropertyModifyWay.Percentage,speed,true);
        if (buffMiniData.changedProperty == FighterProperty.Shield)
            changedShieldValue += value;
        if (buffMiniData.changedProperty == FighterProperty.CooldownPercentage)
            changedAttackSpeedValue += value;
        if (buffMiniData.changedProperty == FighterProperty.Speed)
            changedMoveSpeedValue += value;
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
    
    
    
}
