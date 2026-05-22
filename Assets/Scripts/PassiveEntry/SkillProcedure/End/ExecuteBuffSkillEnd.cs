using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExecuteBuffSkillEnd : SkillEnd {
    
    public BuffData BuffData;
    
    public override void AdditionalProcedure(Fighter influenceFighter, SkillEffect effect, EffectData effectData) {
        if (!effect.Delivery || !effect.Delivery.Caster) return;
        if (effect.Delivery.Caster.TryGetComponent(out Fighter caster)) {
            BuffManager.Instance.AddBuff(caster,influenceFighter, BuffData);
        }
    }
}
