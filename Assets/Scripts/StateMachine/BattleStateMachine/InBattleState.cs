
using UnityEngine;

public class InBattleState : BattleState{

    [SerializeField] private AudioClip InBattleSfx;
    [SerializeField] private AudioClip InBattleMusic;
    
    private VictoryState Victory;
    private DefeatState Defeat;

    protected override void Awake() {
        base.Awake();
        Victory = GetComponent<VictoryState>();
        Defeat = GetComponent<DefeatState>();
    }

    public override void Construct(){

        if (InBattleSfx) { 
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, InBattleSfx);
        }
        
        if(InBattleMusic)
            AudioManager.Instance.SetMainMusic(InBattleMusic);

        BattleUIManager.Instance.heroDetailUI.Hide();
        BattleUIManager.Instance.heroWarehouseUI.Hide();
        foreach (Hero hero in Controller.HeroesInBattle) {
            hero.BattleStart();
        }

        foreach (Enemy enemy in Controller.EnemiesInBattle){
            enemy.BattleStart();
        }
    }

    public override void Transition() {
        // Go to Victory or Defeat
        if (Controller.HeroesInBattle.Count <= 0) {
            Controller.ChangeState(Defeat);
        }

        if (Controller.EnemiesInBattle.Count <= 0) {
            Controller.ChangeState(Victory);
        }
    }
}

