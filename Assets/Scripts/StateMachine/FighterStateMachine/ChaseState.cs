
using UnityEngine;

public class ChaseState : FighterState {
    
    private Fighter ChaseTarget;

    private PatrolState FighterPatrol;
    private SkillState FighterSkill;
    private AttackState FighterAttack;

    private bool IsMoveStop;
    private bool IsFirstFrame = true;

    protected override void Awake(){
        base.Awake();
        FighterPatrol = GetComponent<PatrolState>();
        FighterSkill = GetComponent<SkillState>();
        FighterAttack = GetComponent<AttackState>();
    }

    public override void Construct(){
        IsMoveStop = false;
        IsFirstFrame = true;
        this.ChaseTarget = Controller.AttackTarget;
        Controller.Move.StartMove();
    }

    public override void Execute(){
        // Wait One Frame -> Wait NavMesh Update
        if (IsFirstFrame){
            IsFirstFrame = false;
            return;
        }
        if(this.ChaseTarget) Controller.Move.MoveTo(ChaseTarget.transform.position);
    }

    public override void Destruct() {
        if(IsMoveStop) Controller.Move.StopMove();
    }

    public override void Transition(){
        if (BattleManager.Instance.IsGameOver){
            IsMoveStop = true;
            Controller.FighterAnimator.SetTrigger(AnimationParams.Idle);
            Controller.FighterAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
            Controller.ChangeState(null);
            return;
        }

        if (!this.ChaseTarget){
            Controller.ChangeState(FighterPatrol);
            return;
        }

        float sqrtDistance = (this.ChaseTarget.transform.position - this.transform.position).sqrMagnitude;
        if (sqrtDistance <= Controller.AttackRadius * Controller.AttackRadius){
            IsMoveStop = true;
            if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill()){
                Controller.ChangeState(FighterSkill);
            } else{
                Controller.ChangeState(FighterAttack);
            }
        }
    }
}

