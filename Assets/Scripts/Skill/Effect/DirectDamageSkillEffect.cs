
public class DirectDamageSkillEffect : SkillEffect {
    protected override void Apply(Fighter influenceFighter, EffectData effectData) {
        influenceFighter.BeDamaged(effectData);
    }
}
