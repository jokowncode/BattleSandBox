
public class HealerPatrolState : PatrolState {
    
    public override void Construct() {
        Controller.FighterAnimator.SetTrigger(AnimationParams.Idle);
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

