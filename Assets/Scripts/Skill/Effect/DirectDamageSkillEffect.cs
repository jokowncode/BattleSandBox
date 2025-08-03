
using UnityEngine;

public class DirectDamageSkillEffect : SkillEffect {
    protected override void Apply(Fighter influenceFighter, EffectData effectData) {
        influenceFighter.BeDamaged(effectData);
        //Buff.Instance.AddBuff(influenceFighter);
        //Debug.Log("wow");
        Debug.Log("BuffDataExecute");
        Buff.Instance.AddBuff(influenceFighter,CurrentBuffData);
        
    }
}
