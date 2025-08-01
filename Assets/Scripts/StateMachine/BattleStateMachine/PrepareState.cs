
using UnityEngine;

public class PrepareState : BattleState {

    private InBattleState InBattle;

    protected override void Awake() {
        base.Awake();
        InBattle = GetComponent<InBattleState>();
    }

    public override void Transition() {
        // TODO: According UI
        if (Input.GetKeyDown(KeyCode.Space)) {
            Controller.ChangeState(InBattle);
        }
    }
}
