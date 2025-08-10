using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExecuteBuffSkillEnd : SkillEnd {
    
    public BuffData BuffData;
    public GameObject immediateEffectPrefab;  // 立即效果粒子预制体
    public GameObject tickEffectPrefab;
    
    public override void AdditionalProcedure(Fighter influenceFighter, SkillEffect effect, EffectData effectData)
    {
        if (!effect.Delivery.Caster) return;
        if (!influenceFighter.TryGetComponent(out Buff buff)) {
            buff = influenceFighter.AddComponent<Buff>();
            //buff.transform.position =  influenceFighter.Center.position;
        }
        if(immediateEffectPrefab!=null)
            buff.immediateEffectPrefab = immediateEffectPrefab;
        if(tickEffectPrefab!=null)
            buff.tickEffectPrefab = tickEffectPrefab;
        
        buff.AddBuff(effect.Delivery.Caster.GetComponent<Fighter>(),influenceFighter, BuffData);
        
        // Buff buff = influenceFighter.AddComponent<Buff>();
        // buff.AddBuff(effect.Delivery.Caster.GetComponent<Fighter>(),influenceFighter, BuffData);
    }
}
