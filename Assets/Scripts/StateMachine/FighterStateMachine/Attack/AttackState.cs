
using UnityEngine;

public abstract class AttackState : FighterState{

    [SerializeField] protected ParticleSystem AttackParticle;
    
    protected Fighter AttackTarget;
    private SkillState FighterSkill;
    private float LastAttackTime;
    

    protected override void Awake(){
        base.Awake();
        FighterSkill = GetComponent<SkillState>();
    }

    public override void Construct(){
        this.AttackTarget = Controller.AttackTarget;
        Controller.FighterAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
        LastAttackTime = 0.1f;
    }

    public override void Destruct() {
        if (AttackParticle) {
           AttackParticle.Stop();
        }
    }

    public override void Execute(){
        if (!this.AttackTarget) return;

        Vector3 moveVec = AttackTarget.Center.position - transform.position;
        Controller.Move.ChangeForward(moveVec.x);

        float coolDownTime = Controller.Cooldown;
        if (LastAttackTime > 0.0f && Time.time - LastAttackTime < coolDownTime) return;
        // TODO: After Play Attack Anim -> Attack
        Attack();
        LastAttackTime = Time.time;
    }
    
    protected abstract void Attack();

    public override void Transition(){
        if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill) {
            Controller.ChangeState(FighterSkill);
        }
    }
}
