
using System;
using UnityEngine;

public class CorridorInBattleState : InBattleState {

    [SerializeField] private NextCorridorArea FirstArea;

    public Action OnEnemyBeClear;
    
    protected override void VictoryTransition() {
        if (Controller.EnemiesInBattle.Count <= 0) {
            OnEnemyBeClear?.Invoke();
        }
    }

    protected override void InitializeFighters() {
        FirstArea.Active();
    }
}

