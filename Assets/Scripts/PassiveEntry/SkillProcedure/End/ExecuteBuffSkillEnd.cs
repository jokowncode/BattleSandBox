using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExecuteBuffSkillEnd : SkillEnd {
    
    public BuffData BuffData;
    
    public override void AdditionalProcedure(Fighter influenceFighter, SkillEffect effect, EffectData effectData) {
        if (!effect.Delivery.Caster) return;
        BuffManager.Instance.AddBuff(effect.Delivery.Caster.GetComponent<Fighter>(),influenceFighter, BuffData);
        
        // Buff buff = influenceFighter.AddComponent<Buff>();
        // buff.AddBuff(effect.Delivery.Caster.GetComponent<Fighter>(),influenceFighter, BuffData);
    }
}
