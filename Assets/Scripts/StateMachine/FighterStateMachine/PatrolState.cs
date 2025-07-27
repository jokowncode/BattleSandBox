
using UnityEngine;

public class PatrolState : FighterState {

    [SerializeField] private Transform PatrolPoint;
    
    private AttackState FighterAttack;

    protected override void Awake(){
        base.Awake();
        FighterAttack = GetComponent<AttackState>();
    }

    public override void Construct(){

    }

    public override void Execute(){
        
    }
    
    public override void Transition(){

    }
}
