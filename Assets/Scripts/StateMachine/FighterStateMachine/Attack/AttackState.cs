
using System;
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
    }

    private void Start() {
        Controller.AnimationEvent.OnAttack += OnAttack;
        Controller.AnimationEvent.OnAttackEnd += OnAttackEnd;
    }

    protected virtual void OnAttackEnd() {
        if (IsNeedTarget && !this.AttackTarget) {
            Controller.ChangeState(FighterPatrol);
            return;
        }

        if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill()) {
            Controller.ChangeState(FighterSkill);
        }
    }

    public override void Construct(){
        this.AttackTarget = Controller.AttackTarget;
        Controller.FighterAnimator.SetTrigger(AnimationParams.Attack);
        Controller.FighterAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
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
    
    protected abstract void OnAttack();
    
}
