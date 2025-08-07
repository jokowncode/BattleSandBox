
using UnityEngine;

public class DirectDamageSkillEffect : SkillEffect {
    protected override void Apply(Fighter influenceFighter, EffectData effectData) {
        influenceFighter.BeDamaged(effectData);
#if DEBUG_MODE
        Debug.Log($"{this.Delivery.Caster.name} Cast Skill : {effectData.Value}");
#endif
    }
}
