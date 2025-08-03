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
    
    private float changedSpeedValue;
    private float changedPhysicsAttackValue;
    private float changedMagicAttackValue;
    
    
    public void AddBuff(Fighter target)
    {
        StartCoroutine(BuffRoutine(target));
    }
    
    private IEnumerator BuffRoutine(Fighter target)
    {
        changedSpeedValue = 0f;
        changedPhysicsAttackValue = 0f;
        changedMagicAttackValue = 0f;
        
        TimeRemaining = BuffDetail.duration;
        // 立即应用一次效果
        //ApplyBuffEffects(target);
        CalculateChangedValue(target);
        
        ApplyImmediateBuffEffects(target);
        
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

        if (BuffDetail.isRemoveBuff)
        {
            RemoveBuff(target);
            Debug.Log("Removed buff");
        }
    }
    
    private void ApplyLongTimeBuffEffects(Fighter target = null)
    {
        // 应用生命值变化
        if (BuffDetail.healthChangePerTick != 0)
        {
            DirectDamage(target,BuffDetail.healthChangePerTick);
            Debug.Log("DamageBuff");
        }
        
        // 应用属性修改（示例）
        // PlayerStats stats = Target.GetComponent<PlayerStats>();
        // if (stats)
        // {
        //     stats.ApplySpeedMultiplier(Data.speedMultiplier);
        //     stats.ApplyDamageMultiplier(Data.damageMultiplier);
        // }
    }
    
    private void ApplyImmediateBuffEffects(Fighter target = null)
    {
        if(BuffDetail.speedMultiplier != 1.0f)
        {
            target.Speed += changedSpeedValue;
            Debug.Log("SpeedBuff");
        }
        if (BuffDetail.damageMultiplier != 1.0f)
        {
            target.PhysicsAttack += changedPhysicsAttackValue;
            target.MagicAttack += changedMagicAttackValue;
            Debug.Log("AttackBuff");
        }

    }
    

    public void DirectDamage(Fighter tf,float damage)
    {
        tf.Health -= damage;
    }

    public void LimitedTimeBuff()
    {
        
    }

    public void AddSpeed(Fighter tf, float speed)
    {
        tf.Speed += speed;
    }

    public void DeclineSpeed(Fighter tf, float speed)
    {
        tf.Speed -= speed;
    }

    public void CalculateChangedValue(Fighter target)
    {
        if(BuffDetail.speedMultiplier != 1.0f)
        {
            changedSpeedValue = (BuffDetail.speedMultiplier - 1.0f) * target.Speed;
        }
        if (BuffDetail.damageMultiplier != 1.0f)
        {
            changedPhysicsAttackValue = (BuffDetail.speedMultiplier - 1.0f) * target.PhysicsAttack;
        }
        if (BuffDetail.damageMultiplier != 1.0f)
        {
            changedMagicAttackValue = (BuffDetail.speedMultiplier - 1.0f) * target.MagicAttack;
        }
    }

    public void RemoveBuff(Fighter target)
    {
        if(changedSpeedValue != 0.0f)
        {
            target.Speed -= changedSpeedValue;
            changedSpeedValue = 0.0f;
        }
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
