using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllHeroBuffSkillEnd : SkillEnd {
    
    public BuffData BuffData;
    
    public override void AdditionalProcedure(Fighter influenceFighter, SkillEffect effect, EffectData effectData){
        if (!influenceFighter.TryGetComponent(out Buff buff)) {
            buff = influenceFighter.AddComponent<Buff>();
        }
        
        buff.AddBuff(effect.Delivery.Caster.GetComponent<Fighter>(),influenceFighter, BuffData);
        
        // Buff buff = influenceFighter.AddComponent<Buff>();
        // buff.AddBuff(effect.Delivery.Caster.GetComponent<Fighter>(),influenceFighter, BuffData);
    }
}
