
using System;
using UnityEngine;

public class FighterDeadBattleDefeat : MonoBehaviour {
    private void Awake() {
        if (this.TryGetComponent(out Fighter fighter)) {
            fighter.OnDead += _ => {
                if (BattleManager.Instance) {
                    BattleManager.Instance.BattleDefeat();
                }        
            };
        }
    }
}

