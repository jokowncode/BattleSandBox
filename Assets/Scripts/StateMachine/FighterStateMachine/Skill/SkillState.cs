
using UnityEngine;

public class SkillState : FighterState {

    private Transform AttackTarget;
    private AttackState FighterAttack;

    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
    }
    
    public override void Construct() {
        if (!Controller.FighterSkillCaster.CanCastSkill) return;
        Controller.FighterAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
        this.AttackTarget = Controller.AttackTarget ? Controller.AttackTarget.Center.transform : null;
        Controller.FighterSkillCaster.CastSkill(this.AttackTarget);
    }

    public override void Transition(){
        Controller.ChangeState(FighterAttack);
    }
}

