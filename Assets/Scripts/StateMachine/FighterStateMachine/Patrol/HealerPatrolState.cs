
public class HealerPatrolState : PatrolState {
    
    public override void Execute(){ }

    public override void Transition(){
        if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill()){
            Controller.ChangeState(FighterSkill);
        } else {
            Controller.ChangeState(FighterAttack);
        }
    }
}

