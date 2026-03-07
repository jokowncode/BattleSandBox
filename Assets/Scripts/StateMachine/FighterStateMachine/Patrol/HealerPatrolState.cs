
using UnityEngine;

public class HealerPatrolState : PatrolState {

    private int DefaultAnimatorState;
    
    protected override void Awake() {
        base.Awake();
        this.DefaultAnimatorState = Controller.FighterAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash;
    }

    public override void Construct() {
        AnimatorStateInfo stateInfo = Controller.FighterAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.shortNameHash != DefaultAnimatorState) {
            Controller.FighterAnimator.SetTrigger(AnimationParams.Idle);
        }
    }

    public override void Execute(){ }

    public override void Transition(){
        if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill()){
            Controller.ChangeState(FighterSkill);
        } else if (FighterAttack.CanAttack()) {
            Controller.ChangeState(FighterAttack);
        }
    }
}

