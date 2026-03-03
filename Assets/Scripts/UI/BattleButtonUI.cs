
using System;
using UnityEngine;

public class BattleButtonUI : MonoBehaviour {

    [SerializeField] private GameObject QuitBattle;
    [SerializeField] private GameObject RewindButton;
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private GameObject PausePanel;

    private bool IsPauseBattle = false;

    private void Start() {
        BattleManager.Instance.OnBattleStart += () => {
            this.RewindButton.SetActive(true);
            this.PauseButton.SetActive(true);
            // TODO: FOR TEST -> ALWAYS CAN SKIP BATTLE
            // this.QuitBattle.SetActive(GameManager.Instance.IsTrainBattle);
            this.QuitBattle.SetActive(true);
        };
    }

    public void GoToBigMap(){
        BattleManager.Instance.BattleDefeat();
    }

    public void WinGame() {
        if (this.IsPauseBattle) return;
        BattleManager.Instance.BattleVictory();
    }

    public void RewindBattle() {
        if (this.IsPauseBattle) return;
        BattleManager.Instance.RewindBattle();
    }

    public void PauseBattle() {
        this.IsPauseBattle = !this.IsPauseBattle;
        this.PausePanel.SetActive(this.IsPauseBattle);
        UISelectionManager.Instance.StopTime(this.IsPauseBattle);
    }

}

