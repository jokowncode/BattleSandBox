
using UnityEngine;

public class SkillEndProcedurePassiveEntry : PassiveEntry{

    [SerializeField] private SkillEnd End;

    public override void Construct(Hero hero){
        hero.FighterSkillCaster.AddSkillEnd(this.End);
    }

    public override void Destruct(Hero hero) {
        hero.FighterSkillCaster.RemoveSkillEnd(this.End);
    }
}

