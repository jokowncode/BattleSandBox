
using System;
using UnityEngine;

public class PatrolState : FighterState{

    protected AttackState FighterAttack;
    protected SkillState FighterSkill;
    private ChaseState FighterChase;
    
    private Collider[] SearchTarget;

    public Action<Fighter> OnFindAttackTarget;

    private Fighter PatrolPoint;
    private bool IsMoveStop;
    private bool IsFirstFrame = true;
    private Fighter LastPatrolPoint;

    private bool IsIdle = false;
    
    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
        FighterSkill = GetComponent<SkillState>();
        FighterChase = GetComponent<ChaseState>();
        SearchTarget = new Collider[1];
    }

    public override void Construct() {
        IsMoveStop = false;
        IsFirstFrame = true;
        IsIdle = false;
        Controller.Move.StartMove();
    }

    public override void Execute() {
        // Wait One Frame -> Wait NavMesh Update
        if (IsFirstFrame){
            IsFirstFrame = false;
            return;
        }
        if (BattleManager.Instance.IsGameOver) return;
        if (!this.PatrolPoint || !FormationManager.Instance.ValidTarget(this.PatrolPoint)){
            Func<Fighter, bool> condition = null;
            if (Controller.Type == FighterType.Warrior){
                condition = (Fighter warrior) => FormationManager.Instance.ValidTarget(warrior);
            }
            this.PatrolPoint = BattleManager.Instance.GetNearestFighter(Controller, condition);
        }

        if (!this.PatrolPoint && !this.IsIdle) {
            this.IsIdle = true;
            Controller.Move.MoveTo(this.Controller.transform.position);
            Controller.FighterAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
            Controller.FighterAnimator.SetTrigger(AnimationParams.Idle);
        }

        if (this.PatrolPoint) {
            this.IsIdle = false;
            Vector3 finalPos = Controller.Type == FighterType.Warrior ? 
                FormationManager.Instance.GetFormationPosition(this.PatrolPoint, Controller.AttackRadius) : 
                this.PatrolPoint.transform.position;
            Controller.Move.MoveTo(finalPos);
        }
    }

    public override void Destruct() {
        PatrolPoint = null;
        if(IsMoveStop) Controller.Move.StopMove();
    }

    private bool IsValid(Fighter target) {
        return this.Controller.Type != FighterType.Warrior || FormationManager.Instance.ValidTarget(target);
    }

    public override void Transition() {
        if (BattleManager.Instance.IsGameOver){
            IsMoveStop = true;
            Controller.FighterIdle();
            return;
        }

        int result = Physics.OverlapSphereNonAlloc(transform.position, Controller.AttackRadius, 
            SearchTarget, LayerMask.GetMask(Controller.AttackTargetType.ToString()));
        if (result != 0 && SearchTarget[0].gameObject.TryGetComponent(out Fighter attackTarget)
            && IsValid(attackTarget)) {
            IsMoveStop = true;
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
            if (result != 0 && SearchTarget[0].gameObject.TryGetComponent(out Fighter chaseTarget)
                && IsValid(chaseTarget)) {
                OnFindAttackTarget?.Invoke(chaseTarget);
                Controller.ChangeState(FighterChase);
            }
        }
    }
}
