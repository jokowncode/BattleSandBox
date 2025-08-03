
using System;
using UnityEngine;

public class PatrolState : FighterState{

    protected AttackState FighterAttack;
    protected SkillState FighterSkill;
    private ChaseState FighterChase;
    
    private Collider[] SearchTarget;

    public Action<Fighter> OnFindAttackTarget;

    private Fighter PatrolPoint;
    
    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
        FighterSkill = GetComponent<SkillState>();
        FighterChase = GetComponent<ChaseState>();
        SearchTarget = new Collider[1];
    }

    public override void Construct() {
        this.PatrolPoint = BattleManager.Instance.GetRandomFighter(Controller.AttackTargetType);
    }

    public override void Execute() {
        if (BattleManager.Instance.IsGameOver) return;
        if (!this.PatrolPoint) {
            this.PatrolPoint = BattleManager.Instance.GetRandomFighter(Controller.AttackTargetType);
        }
        if (!this.PatrolPoint) return;
        // Vector3 dir = (this.PatrolPoint.transform.position - this.transform.position).normalized;
        // Controller.Move.MoveByDirection(dir);
        Controller.Move.MoveTo(this.PatrolPoint.transform.position);
    }

    public override void Destruct() {
        Controller.Move.StopMove();
    }

    public override void Transition() {

        if (BattleManager.Instance.IsGameOver) {
            Controller.FighterAnimator.SetTrigger(AnimationParams.Idle);
            Controller.FighterAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
            Controller.ChangeState(null);
            return;
        }

        int result = Physics.OverlapSphereNonAlloc(transform.position, Controller.AttackRadius, 
            SearchTarget, LayerMask.GetMask(Controller.AttackTargetType.ToString()));
        if (result != 0 && SearchTarget[0].gameObject.TryGetComponent(out Fighter attackTarget)) {
            OnFindAttackTarget?.Invoke(attackTarget);
            if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill()){
                Controller.ChangeState(FighterSkill);
            } else{
                Controller.ChangeState(FighterAttack);
            }
            return;
        }

        if (this.FighterChase && Controller.Type == FighterType.Warrior) {
            result = Physics.OverlapSphereNonAlloc(transform.position, 10.0f, 
                SearchTarget, LayerMask.GetMask(Controller.AttackTargetType.ToString()));
            if (result != 0 && SearchTarget[0].gameObject.TryGetComponent(out Fighter chaseTarget)) {
                OnFindAttackTarget?.Invoke(chaseTarget);
                Controller.ChangeState(FighterChase);
            }
        }
    }
}
