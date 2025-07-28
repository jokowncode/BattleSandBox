
using System;
using UnityEngine;

public class PatrolState : FighterState {
    
    private AttackState FighterAttack;
    private Collider[] SearchAttackTarget;

    public Action<Fighter> OnFindAttackTarget;
    
    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
        SearchAttackTarget = new Collider[1];
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
            Controller.ChangeState(FighterAttack);
        }
    }
}
