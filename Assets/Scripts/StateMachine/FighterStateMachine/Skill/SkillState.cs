
using System;
using UnityEngine;

public class SkillState : FighterState {
    
    private Transform AttackTarget;
    private AttackState FighterAttack;

    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
    }

    private void Start() {
        Controller.AnimationEvent.OnSkill += OnSkill;
        Controller.AnimationEvent.OnSkillEnd += OnSkillEnd;
    }

    private void OnSkillEnd() {
        Controller.ChangeState(FighterAttack);
    }

    private void OnSkill() {
        this.AttackTarget = Controller.AttackTarget ? Controller.AttackTarget.Center.transform : null;
        Controller.FighterSkillCaster.CastSkill(this.AttackTarget);
    }

    public override void Construct() {
        if (!Controller.FighterSkillCaster.CanCastSkill()) return;
        Controller.FighterAnimator.SetTrigger(AnimationParams.Skill);
    }
}

