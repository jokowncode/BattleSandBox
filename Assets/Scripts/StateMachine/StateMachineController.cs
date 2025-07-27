
using System;
using UnityEngine;

public class StateMachineController : MonoBehaviour {

    private State CurrentState;

    public void ChangeState(State newState) {
        if(CurrentState != null) CurrentState.Destruct();
        CurrentState = newState;
        if(CurrentState != null) CurrentState.Construct();
    }

    protected virtual void Update() {
        if (CurrentState) {
            CurrentState.Execute();
            CurrentState.Transition();
        }
    }
}
