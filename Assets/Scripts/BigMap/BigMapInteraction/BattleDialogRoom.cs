
using System;
using UnityEngine;

public class BattleDialogRoom : BattleRoom {

    [SerializeField] private DialogGraph BattleStartDialog;
    [SerializeField] private DialogGraph BattleLoseDialog;
    [SerializeField] private DialogGraph BattleVictoryDialog;

    enum State {
        PreBattle,
        Victory,
        Lose
    }

    private State CurrentState;
    
    private void OnDialogEnded() {
        DialogManager.Instance.OnDialogEnded -= OnDialogEnded;
        if (CurrentState == State.PreBattle) {
            GameManager.Instance.GoToBattle(this.Data);
            return;
        }

        if (CurrentState == State.Victory) {
            OnVictoryDialogEnded();
            return;
        }

        if (CurrentState == State.Lose) {
            OnLoseDialogEnded();
        }
    }

    protected virtual void OnVictoryDialogEnded() { }
    protected virtual void OnLoseDialogEnded() { }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        this.CurrentState = State.PreBattle;
        if (GameManager.Instance.IsBattleEnd) {
            if (GameManager.Instance.IsBattleVictory && this.BattleVictoryDialog) {
                this.CurrentState = State.Victory;
                DialogManager.Instance.OnDialogEnded += OnDialogEnded;
                DialogManager.Instance.PlayNewDialog(this.gameObject, this.BattleVictoryDialog);
            }else if (!GameManager.Instance.IsBattleVictory && this.BattleLoseDialog) {
                this.CurrentState = State.Lose;
                DialogManager.Instance.OnDialogEnded += OnDialogEnded;
                DialogManager.Instance.PlayNewDialog(this.gameObject, this.BattleLoseDialog);
            }
        }
    }

    protected override void Interaction() {
        if (this.IsEnd) return;
        if (DialogManager.Instance.IsInDialog) return;
        if (this.CurrentState == State.PreBattle && BattleStartDialog) {
            DialogManager.Instance.OnDialogEnded += OnDialogEnded;
            DialogManager.Instance.PlayNewDialog(this.gameObject, this.BattleStartDialog);
        } else {
            GameManager.Instance.GoToBattle(this.Data);
        }
    }
}

