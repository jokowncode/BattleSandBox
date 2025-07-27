
using UnityEngine;

public abstract class AttackState : FighterState{

    [SerializeField] protected ParticleSystem AttackParticle;
    
    protected Enemy AttackTarget;
    
    // protected Vector3 MoveVec => AttackTarget ? AttackTarget.position - transform.position : Vector3.zero;

    private PatrolState FighterPatrol;
    private SkillState FighterSkill;

    private float[] LastSkillTime;
    private float LastAttackTime;
    

    protected override void Awake(){
        base.Awake();
        FighterPatrol = GetComponent<PatrolState>();
        FighterSkill = GetComponent<SkillState>();

        // LastSkillTime = new float[Controller.Data.PetSkills.Length];
    }

    public override void Construct(){
        // TODO: Get Attack Target
        // this.AttackTarget = Controller.AttackTarget;
    }

    public override void Destruct() {
        // if (AttackParticle) {
        //    AttackParticle.Stop();
        // }
    }

    public override void Execute(){
        // TODO: Attack
        /*if (!this.AttackTarget) return;

        Vector3 moveVec = AttackTarget.Aim.position - transform.position;
        Controller.PetMove.ChangeForward(moveVec.x);

        float coolDownTime = Controller.Data.AutoAttackCooldownTime;
        if (LastAttackTime > 0.0f && Time.time - LastAttackTime < coolDownTime) return;
        Attack();
        LastAttackTime = Time.time;*/
    }

    // Specifical Attack Way
    protected abstract void Attack();

    public override void Transition(){
        // TODO: When Skill Cooldown end -> Change To Skill State
    }
}
