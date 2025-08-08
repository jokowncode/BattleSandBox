
using UnityEngine;

public class SkillMiddleProcedurePassiveEntry : PassiveEntry{

    [SerializeField] private SkillMiddle Middle;

    public override void Construct(Hero hero){
        hero.FighterSkillCaster.AddSkillMiddle(this.Middle);
    }

    public override void Destruct(Hero hero) {
        hero.FighterSkillCaster.RemoveSkillMiddle(this.Middle);
    }
}

