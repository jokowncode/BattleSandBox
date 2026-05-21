
using System;
using UnityEngine;

public class InBattleState : BattleState{

    [SerializeField] private AudioClip InBattleSfx;
    
    private VictoryState Victory;
    private DefeatState Defeat;

    protected override void Awake() {
        base.Awake();
        Victory = GetComponent<VictoryState>();
        Defeat = GetComponent<DefeatState>();
    }

    public override void Construct(){

        if (InBattleSfx) { 
            AudioManager.Instance.PlaySfx(InBattleSfx);
        }
        
        if(Controller.Data.BattleBGM)
            AudioManager.Instance.SetMainMusic(Controller.Data.BattleBGM);

        BattleUIManager.Instance.heroDetailUI.Hide();
        BattleUIManager.Instance.heroWarehouseUI.Hide();
        
        InitializeFighters();
    }

    protected virtual void InitializeFighters() {
        Controller.StartBattleInRound();
    }

    protected virtual void VictoryTransition() {
        if (Controller.EnemiesInBattle.Count <= 0) {
            Controller.ChangeState(Victory);  
        }
    }

    public override void Transition() {
        // Go to Victory or Defeat
        if (Controller.HeroesInBattle.Count <= 0) {
            Controller.ChangeState(Defeat);
            return;
        }
        VictoryTransition();
    }
}

