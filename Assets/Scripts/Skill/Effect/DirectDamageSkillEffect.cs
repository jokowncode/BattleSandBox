
public class DirectDamageSkillEffect : SkillEffect {
    protected override void Apply(Fighter influenceFighter, SkillEffectData skillEffect) {
        influenceFighter.BeDamaged(skillEffect);
    }
}
