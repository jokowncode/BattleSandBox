
using System;
using UnityEngine;

public class BattleDefeatRecoverAllHeroHealth : MonoBehaviour {
    private void Awake() {
        if (TryGetComponent(out BattleRoom battleRoom)) {
            battleRoom.OnDefeat += () => {
                SaveMapManager.Instance.RecoverAllHeroHealth();
            };
        }
    }
}

