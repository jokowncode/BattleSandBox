using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuffUtils
{
    public static float GetProperty(Fighter ft,FighterProperty prop)
    {
        return ReflectionTools.GetObjectProperty<float>(prop.ToString(), ft);
    }
    
    public static void InitializeBuffData(Fighter fighter,ref BuffData buffData)
    {
        // 参数校验
        if (fighter == null)
        {
            Debug.LogError("初始化Buff时fighter参数为空");
            return;
        }

        // 确保buffData已初始化
        if (buffData == null)
        {
            buffData = new BuffData(); // 或者默认构造函数
            Debug.LogWarning("传入的buffData未初始化，已创建默认实例");
        }
        
        // float healthBasic = fighter.GetInitialData().Health;
        // float attackSpeedBasic = fighter.FighterAnimator.GetFloat(AnimationParams.AttackAnimSpeedMultiplier);
        // float moveSpeedBasic = fighter.GetInitialData().Speed;
        // float physicsAttackBasic = fighter.GetInitialData().PhysicsAttack;
        // float magicAttackBasic = fighter.GetInitialData().MagicAttack;
        
        foreach (var buffMiniData in buffData.immediateEffectBuff)
        {
            //InitialPrams(ref buffMiniData);
            
            if(buffMiniData.basicRef==BasicRef.Caster)
            {
                //if()
                buffMiniData.value = ReflectionTools.GetObjectProperty<float>(buffMiniData.refProperty.ToString(), fighter) * buffMiniData.changedValue; 
            }
            else
            {
                buffMiniData.value = buffMiniData.changedValue;   
            }
        }
        foreach (var buffMiniData in buffData.longTimeEffectBuff)
        {
            //InitialPrams(ref buffMiniData);
            if(buffMiniData.basicRef==BasicRef.Caster)
            {
                buffMiniData.value = ReflectionTools.GetObjectProperty<float>(buffMiniData.refProperty.ToString(), fighter) * buffMiniData.changedValue; 
            }
            else
            {
                buffMiniData.value = buffMiniData.changedValue; 
            }
        }
        foreach (var buffMiniData in buffData.lastEffectBuff)
        {
            //InitialPrams(ref buffMiniData);
            if(buffMiniData.basicRef==BasicRef.Caster)
            {
                buffMiniData.value = ReflectionTools.GetObjectProperty<float>(buffMiniData.refProperty.ToString(), fighter) * buffMiniData.changedValue; 
            }
            else
            {
                buffMiniData.value = buffMiniData.changedValue;   
            }
        }
    }

    static void InitialPrams(ref BuffMiniData buffMiniData)
    {
        buffMiniData.value = 0f;
    }
}
