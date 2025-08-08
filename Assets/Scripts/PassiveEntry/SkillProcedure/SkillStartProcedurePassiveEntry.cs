
using UnityEngine;

public class SkillStartProcedurePassiveEntry : PassiveEntry{

    [SerializeField] private SkillStart Start;

    public override void Construct(Hero hero){
        hero.FighterSkillCaster.AddSkillStart(this.Start);
    }

    public override void Destruct(Hero hero) {
        hero.FighterSkillCaster.RemoveSkillStart(this.Start);
    }
}

