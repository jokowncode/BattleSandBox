using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Buff))]
public class ExecuteBuffSkillEnd : SkillEnd {
    public BuffData BuffData;
    
    public override void AdditionalProcedure(Fighter influenceFighter, SkillEffect effect, EffectData effectData){
        this.GetComponent<Buff>().AddBuff(effect.Delivery.Caster.GetComponent<Fighter>(),influenceFighter, BuffData);
    }
}
