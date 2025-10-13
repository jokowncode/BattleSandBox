
using System;
using UnityEngine;

public class BattleDialogRoom : BattleRoom {

    [SerializeField] private StoryDialogData[] BattleStartDialog;
    [SerializeField] private StoryDialogData[] BattleLoseDialog;
    [SerializeField] private StoryDialogData[] BattleVictoryDialog;

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
            if (GameManager.Instance.IsBattleVictory && this.BattleVictoryDialog is {Length: > 0}) {
                this.CurrentState = State.Victory;
                DialogManager.Instance.OnDialogEnded += OnDialogEnded;
                DialogManager.Instance.PlayNewDialog(this.BattleVictoryDialog, null);
            }else if (!GameManager.Instance.IsBattleVictory && this.BattleLoseDialog is {Length: > 0}) {
                this.CurrentState = State.Lose;
                DialogManager.Instance.OnDialogEnded += OnDialogEnded;
                DialogManager.Instance.PlayNewDialog(this.BattleLoseDialog, null);
            }
        }
    }

    protected override void Interaction() {
        if (this.IsEnd) return;
        if (this.CurrentState == State.PreBattle && BattleStartDialog is { Length: > 0 }) {
            DialogManager.Instance.OnDialogEnded += OnDialogEnded;
            DialogManager.Instance.PlayNewDialog(this.BattleStartDialog, null);
        } else {
            GameManager.Instance.GoToBattle(this.Data);
        }
    }
}

