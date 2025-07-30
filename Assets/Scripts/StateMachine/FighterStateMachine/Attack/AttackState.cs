
using UnityEngine;

public abstract class AttackState : FighterState{
    

    [SerializeField] protected ParticleSystem AttackParticle;
    
    protected Fighter AttackTarget;
    protected SkillState FighterSkill;
    protected bool IsNeedTarget = true;
    
    private PatrolState FighterPatrol;
    
    protected override void Awake(){
        base.Awake();
        FighterSkill = GetComponent<SkillState>();
        FighterPatrol = GetComponent<PatrolState>();

        GetComponentInChildren<FighterAnimationEvent>().OnAttack += Attack;
    }

    public override void Construct(){
        this.AttackTarget = Controller.AttackTarget;
        Controller.FighterAnimator.SetTrigger(AnimationParams.Attack);
    }

    public override void Destruct() {
        if (AttackParticle) {
           AttackParticle.Stop();
        }
    }

    public override void Execute(){
        if (IsNeedTarget && !this.AttackTarget) return;

        if (this.AttackTarget){
            Vector3 moveVec = AttackTarget.Center.position - transform.position;
            Controller.Move.ChangeForward(moveVec.x);
        }
    }
    
    protected abstract void Attack();

    public override void Transition(){
        if (IsNeedTarget && !this.AttackTarget) {
            Controller.ChangeState(FighterPatrol);
            return;
        }

        if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill) {
           // Controller.ChangeState(FighterSkill);
        }
    }
}
