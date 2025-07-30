
using System;
using UnityEngine;

public class PatrolState : FighterState {
    
    protected AttackState FighterAttack;
    protected SkillState FighterSkill;
    
    private Collider[] SearchAttackTarget;

    public Action<Fighter> OnFindAttackTarget;
    
    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
        FighterSkill = GetComponent<SkillState>();
        SearchAttackTarget = new Collider[1];
    }

    public override void Construct(){
        Controller.FighterAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
    }

    public override void Execute() {
        // TODO: Direction
        Controller.Move.MoveByDirection(Vector3.right);
    }
    
    public override void Transition() {
        int result = Physics.OverlapSphereNonAlloc(transform.position, Controller.AttackRadius, 
            SearchAttackTarget, LayerMask.GetMask(Controller.AttackTargetType.ToString()));
        if (result != 0 && SearchAttackTarget[0].gameObject.TryGetComponent(out Fighter fighter)) {
            OnFindAttackTarget?.Invoke(fighter);
            if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill){
                Controller.ChangeState(FighterSkill);
            } else{
                Controller.ChangeState(FighterAttack);
            }
        }
    }
}
