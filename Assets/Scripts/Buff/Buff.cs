using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public static Buff Instance;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public BuffData BuffDetail;
    //public FighterData EffectProp;
    //public Fighter Target { get; }
    public float TimeRemaining { get; private set; }

    private float changedShieldValue;
    private float changedMoveSpeedValue;
    private float changedAttackSpeedValue;
    private float changedPhysicsAttackValue;
    private float changedMagicAttackValue;
    
    
    public void AddBuff(Fighter target,BuffData buffData)
    {
        StartCoroutine(BuffRoutine(target,buffData));
    }
    
    private IEnumerator BuffRoutine(Fighter target,BuffData buffData)
    {
        changedShieldValue = 0f;
        changedMoveSpeedValue = 0f;
        changedAttackSpeedValue = 0f;
        changedPhysicsAttackValue = 0f;
        changedMagicAttackValue = 0f;
        
        TimeRemaining = BuffDetail.duration;
        
        foreach (var buffMiniData in buffData.immediateEffectBuff)
        {
            CalculateChangedValue(target,buffMiniData);
        }

        ApplyImmediateBuffEffects(target, buffData);
        
        if (BuffDetail.tickInterval > 0)
        {
            // 间隔触发效果
            WaitForSeconds wait = new WaitForSeconds(BuffDetail.tickInterval);
            
            while (TimeRemaining > 0)
            {
                yield return wait;
                TimeRemaining -= BuffDetail.tickInterval;
                ApplyLongTimeBuffEffects(target);
            }
        }
        else
        {
            // 单次效果，等待持续时间结束
            yield return new WaitForSeconds(BuffDetail.duration);
            TimeRemaining = 0;
        }

        // if (BuffDetail.isRemoveBuff)
        // {
        //     RemoveBuff(target);
        //     Debug.Log("Removed buff");
        // }
        //yield return null;
    }
    
    private void ApplyLongTimeBuffEffects(Fighter target = null)
    {
        // 应用生命值变化
        // if (BuffDetail.healthChangePerTick != 0)
        // {
        //     DirectDamage(target,BuffDetail.healthChangePerTick);
        //     Debug.Log("DamageBuff");
        // }
        
        // 应用属性修改（示例）
        // PlayerStats stats = Target.GetComponent<PlayerStats>();
        // if (stats)
        // {
        //     stats.ApplySpeedMultiplier(Data.speedMultiplier);
        //     stats.ApplyDamageMultiplier(Data.damageMultiplier);
        // }
    }
    
    private void ApplyImmediateBuffEffects(Fighter target = null,BuffData buffData = null)
    {
        // 立即应用一次效果
        //ApplyBuffEffects(target);
        if (changedShieldValue != 0)
            target.Shield = changedShieldValue;
        if(changedAttackSpeedValue!=0)
            target.PropertyChange(FighterProperty.CooldownPercentage,PropertyModifyWay.Value,changedAttackSpeedValue,changedAttackSpeedValue>0?true:false);
        if(changedMoveSpeedValue != 0)
            target.PropertyChange(FighterProperty.Speed,PropertyModifyWay.Value,changedMoveSpeedValue,changedMoveSpeedValue>0?true:false);
        if(changedPhysicsAttackValue !=0)
            target.PropertyChange(FighterProperty.PhysicsAttack,PropertyModifyWay.Value,changedPhysicsAttackValue,changedPhysicsAttackValue>0?true:false);
        if(changedMagicAttackValue !=0)
            target.PropertyChange(FighterProperty.MagicAttack,PropertyModifyWay.Value,changedMagicAttackValue,changedMagicAttackValue>0?true:false);
    }
    

    public void DirectDamage(Fighter tf,float damage)
    {
        tf.Health -= damage;
    }

    public void LimitedTimeBuff()
    {
        
    }

    public void AddMoveSpeed(Fighter tf, float speed)
    {
        tf.Speed += speed;
    }

    public void DeclineMoveSpeed(Fighter tf, float speed)
    {
        tf.Speed -= speed;
    }

    public void CalculateChangedValue(Fighter target,BuffMiniData buffMiniData)
    {
        if (buffMiniData.basicRef == BasicRef.Target)
        {
            if(buffMiniData.healthValue != 0)
                changedShieldValue = buffMiniData.healthValue * target.Health;
            if (buffMiniData.moveSpeedValue != 0)
                changedMoveSpeedValue = buffMiniData.moveSpeedValue * target.Speed;
            if(buffMiniData.attackSpeedValue !=0)
                changedAttackSpeedValue = buffMiniData.attackSpeedValue * target.FighterAnimator.GetFloat(AnimationParams.AttackAnimSpeedMultiplier);
            if (buffMiniData.damageValue != 0)
            {
                changedMagicAttackValue = buffMiniData.damageValue * target.PhysicsAttack;
                changedMagicAttackValue = buffMiniData.damageValue * target.MagicAttack;
            }
        }
        else
        {
            if(buffMiniData.healthValue != 0)
                changedShieldValue = buffMiniData.healthValue;
            if (buffMiniData.moveSpeedValue != 0)
                changedMoveSpeedValue = buffMiniData.moveSpeedValue;
            if(buffMiniData.attackSpeedValue !=0)
                changedAttackSpeedValue = buffMiniData.attackSpeedValue;
            if (buffMiniData.damageValue != 0)
            {
                changedMagicAttackValue = buffMiniData.damageValue;
                changedMagicAttackValue = buffMiniData.damageValue;
            }
        }
        //target.HeroPropertyChange(FighterProperty.CooldownPercentage,PropertyModifyWay.Percentage,speed,true);
    }

    public void AddAttackSpeed(Hero tf, float speed)
    {
        tf.HeroPropertyChange(FighterProperty.CooldownPercentage,PropertyModifyWay.Percentage,speed,true);
    }

    public void RemoveBuff(Fighter target)
    {
        // if(changedSpeedValue != 0.0f)
        // {
        //     target.Speed -= changedSpeedValue;
        //     changedSpeedValue = 0.0f;
        // }
        if (changedPhysicsAttackValue != 0.0f)
        {
            target.PhysicsAttack -= changedPhysicsAttackValue;
            changedPhysicsAttackValue = 0.0f;
        }
        if (changedMagicAttackValue != 0.0f)
        {
            target.MagicAttack -= changedMagicAttackValue;
            changedMagicAttackValue = 0.0f;
        }
    }
    
    
    
}
