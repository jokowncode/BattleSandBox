
using System;
using UnityEngine;

public class StateMachineController : MonoBehaviour {

    protected State CurrentState;

    public void ChangeState(State newState) {
        if(CurrentState) CurrentState.Destruct();
        CurrentState = newState;
        if(CurrentState) CurrentState.Construct();
    }

    protected virtual void Update(){
        if (!CurrentState) return;
        CurrentState.Execute();
        CurrentState.Transition();
    }
}
