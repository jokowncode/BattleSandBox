
using System;
using UnityEngine;

public class StateMachineController : MonoBehaviour {

    private State CurrentState;

    public void ChangeState(State newState) {
        if(CurrentState) CurrentState.Destruct();
        CurrentState = newState;
        if(CurrentState) CurrentState.Construct();
    }

    private void Update(){
        if (!CurrentState) return;
        CurrentState.Execute();
        CurrentState.Transition();
    }
}
