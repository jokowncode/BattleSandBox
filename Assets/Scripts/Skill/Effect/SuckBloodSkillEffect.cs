
public class SuckBloodSkillEffect : DirectDamageSkillEffect {
    protected override void Apply(Fighter influenceFighter, EffectData effectData) {
        base.Apply(influenceFighter, effectData);
        if (this.Delivery.Caster.TryGetComponent(out Fighter fighter)) {
            fighter.BeHealed(new EffectData {
                Value = effectData.Value,
                NotShowParticle = false
            });
        }
    }
}

