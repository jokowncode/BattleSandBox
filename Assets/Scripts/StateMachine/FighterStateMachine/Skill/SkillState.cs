
using System;
using UnityEngine;

public class SkillState : FighterState {
    
    private Transform AttackTarget;
    private AttackState FighterAttack;
    private PatrolState FighterPatrol;

    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
        FighterPatrol = GetComponent<PatrolState>();
    }

    private void Start() {
        Controller.AnimationEvent.OnSkill += OnSkill;
        Controller.AnimationEvent.OnSkillEnd += OnSkillEnd;
    }

    private void OnSkillEnd() {
        if (FighterAttack.CanAttack()) {
            Controller.ChangeState(FighterAttack);
        } else {
            Controller.ChangeState(FighterPatrol);   
        }
    }

    private void OnSkill() {
        this.AttackTarget = Controller.AttackTarget ? Controller.AttackTarget.Center : null;
        Controller.FighterSkillCaster.CastSkill(this.AttackTarget);
    }

    public override void Construct() {
        if (!Controller.FighterSkillCaster.CanCastSkill()) return;
        Controller.FighterAnimator.SetTrigger(AnimationParams.Skill);
        Controller.FighterAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
    }
}

