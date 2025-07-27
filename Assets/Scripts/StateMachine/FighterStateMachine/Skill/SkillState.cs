
using UnityEngine;

public class SkillState : FighterState {

    private Transform AttackTarget;
    private AttackState FighterAttack;

    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
    }
    
    public override void Construct() {
        if (!Controller.AttackTarget) return;
        this.AttackTarget = Controller.AttackTarget.Center.transform;
        Controller.FighterSkillCaster.CastSkill(this.AttackTarget);
    }

    public override void Transition(){
        Controller.ChangeState(FighterAttack);
    }
}

