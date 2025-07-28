
using UnityEngine;

public class SkillState : FighterState {

    private Transform AttackTarget;
    private AttackState FighterAttack;

    private bool IsChange = false;

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
        // TODO: After Play Skill After-Anim -> change to AttackState
        // Controller.ChangeState(FighterAttack);
        if (IsChange) return;
        IsChange = true;
        Invoke(nameof(ChangeState), 2.0f);
    }

    private void ChangeState(){
        IsChange = false;
        Controller.ChangeState(this.FighterAttack);
    }
}

