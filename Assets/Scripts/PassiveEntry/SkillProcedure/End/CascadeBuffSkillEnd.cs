

using UnityEngine;

public class CascadeBuffSkillEnd : SkillEnd {

    [SerializeField] private BuffData CascadeBuff;
    [SerializeField] private uint NextBuffCount;
    [SerializeField] private BuffData NextBuff;
    
    public override void AdditionalProcedure(Fighter influenceFighter, SkillEffect effect, EffectData _) {
        if (!this.CascadeBuff) return;
        if (!effect.Delivery.Caster.TryGetComponent(out Fighter caster)) return;
        BuffManager.Instance.AddBuff(caster, influenceFighter, this.CascadeBuff);
        if (BuffManager.Instance.TryGetFighterBuffCount(influenceFighter, this.CascadeBuff.CascadeType,
                out int count) && count >= this.NextBuffCount && this.NextBuff) {
            BuffManager.Instance.AddBuff(caster, influenceFighter, this.NextBuff);
        }
    }
}

