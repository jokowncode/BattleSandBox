
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
        Controller.Move.StartMove();
    }

    public override void Execute() {
        // Wait One Frame -> Wait NavMesh Update
        if (IsFirstFrame){
            IsFirstFrame = false;
            return;
        }
        if (BattleManager.Instance.IsGameOver) return;

        /*if (!this.PatrolPoint) {
            this.PatrolPoint = BattleManager.Instance.GetRandomFighter(Controller.AttackTargetType);
        }

        if (this.PatrolPoint){
            Controller.Move.MoveTo(this.PatrolPoint.transform.position);
        }*/
        
        if (!this.PatrolPoint){
            Func<Fighter, bool> condition = null;
            if (Controller.Type == FighterType.Warrior){
                condition = (Fighter warrior) => FormationManager.Instance.ValidTarget(warrior);
            }
            this.PatrolPoint = BattleManager.Instance.GetRandomFighter(Controller.AttackTargetType, condition);
            this.LastPatrolPoint = this.PatrolPoint;
        }

        if (this.PatrolPoint){
            Vector3 finalPos = Controller.Type == FighterType.Warrior ? 
                FormationManager.Instance.GetFormationPosition(this.PatrolPoint, this.LastPatrolPoint, Controller.AttackRadius) : 
                this.PatrolPoint.transform.position;
            Controller.Move.MoveTo(finalPos);
        }
    }

    public override void Destruct() {
        if(IsMoveStop) Controller.Move.StopMove();
    }

    public override void Transition() {

        if (BattleManager.Instance.IsGameOver){
            IsMoveStop = true;
            Controller.FighterIdle();
            return;
        }

        int result = Physics.OverlapSphereNonAlloc(transform.position, Controller.AttackRadius, 
            SearchTarget, LayerMask.GetMask(Controller.AttackTargetType.ToString()));
        if (result != 0 && SearchTarget[0].gameObject.TryGetComponent(out Fighter attackTarget)) {
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
            if (result != 0 && SearchTarget[0].gameObject.TryGetComponent(out Fighter chaseTarget)) {
                OnFindAttackTarget?.Invoke(chaseTarget);
                Controller.ChangeState(FighterChase);
            }
        }
    }
}
