
using System;
using UnityEngine;

public class FighterDeadBattleDefeat : MonoBehaviour {
    private void OnDestroy() {
        if (BattleManager.Instance) {
            BattleManager.Instance.BattleDefeat();
        }
    }
}

