
using UnityEngine;

public class ChaseState : FighterState {
    
    private Fighter ChaseTarget;

    private PatrolState FighterPatrol;
    private SkillState FighterSkill;
    private AttackState FighterAttack;

    protected override void Awake(){
        base.Awake();
        FighterPatrol = GetComponent<PatrolState>();
        FighterSkill = GetComponent<SkillState>();
        FighterAttack = GetComponent<AttackState>();
    }

    public override void Construct(){
        this.ChaseTarget = Controller.AttackTarget;
    }

    public override void Execute(){
        if (BattleManager.Instance.IsGameOver) return;
        if (!ChaseTarget) return;

        Vector3 dir = (ChaseTarget.transform.position - this.transform.position).normalized;
        Controller.Move.MoveByDirection(dir);
    }

    public override void Transition(){
        if (BattleManager.Instance.IsGameOver) {
            Controller.FighterAnimator.SetTrigger(AnimationParams.Idle);
            Controller.ChangeState(null);
            return;
        }

        if (!this.ChaseTarget){
            Controller.ChangeState(FighterPatrol);
            return;
        }

        float sqrtDistance = (this.ChaseTarget.transform.position - this.transform.position).sqrMagnitude;
        if (sqrtDistance <= Controller.AttackRadius * Controller.AttackRadius) {
            if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill()){
                Controller.ChangeState(FighterSkill);
            } else{
                Controller.ChangeState(FighterAttack);
            }
        }
    }
}

