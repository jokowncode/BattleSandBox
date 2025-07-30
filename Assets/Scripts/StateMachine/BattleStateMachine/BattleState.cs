
using System;

public class BattleState : State {

    protected BattleManager Controller;

    protected virtual void Awake() {
        Controller = GetComponent<BattleManager>();
    }
}

