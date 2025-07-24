
using UnityEngine;

public class ChaseState : State{

    private Transform AttackTarget;
    private Vector3 MoveVec => AttackTarget ? AttackTarget.position - transform.position : Vector3.zero;

    private AttackState FighterAttack;

    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
    }

    public override void Construct(){
        // TODO: Get Attack Target
        // this.AttackTarget = Controller.AttackTarget.transform;
    }

    public override void Execute(){
        // TODO: Move To Attack Target
        // if (!this.AttackTarget) return;
        // Controller.PetMove.MoveByDirection(MoveVec.normalized);
        // this.transform.position += Controller.MoveSpeed * Time.deltaTime * MoveVec.normalized;
    }
    
    public override void Transition(){
        // TODO: Enter Attack Radius -> Change To Attack State
        // if(!AttackTarget) Controller.ChangeState(null);
        // float distance = Controller.Data.AttackRadius;
        // if (MoveVec.sqrMagnitude <= distance * distance){
        //    Controller.ChangeState(HeroAttack);
        //}
    }
}
