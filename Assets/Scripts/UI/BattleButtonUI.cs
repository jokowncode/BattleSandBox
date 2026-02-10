
using UnityEngine;

public class BattleButtonUI : MonoBehaviour {

    public void GoToBigMap(){
        BattleManager.Instance.BattleDefeat();
    }

    public void WinGame() {
        BattleManager.Instance.BattleVictory();
    }

    public void RewindBattle() {
        BattleManager.Instance.RewindBattle();
    }

}

