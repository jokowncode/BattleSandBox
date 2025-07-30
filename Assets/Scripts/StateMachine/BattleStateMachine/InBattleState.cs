
public class InBattleState : BattleState {

    private VictoryState Victory;
    private DefeatState Defeat;

    protected override void Awake() {
        base.Awake();
        Victory = GetComponent<VictoryState>();
        Defeat = GetComponent<DefeatState>();
    }

    public override void Construct() {
        foreach (Hero hero in Controller.HeroesInBattle) {
            hero.BattleStart();
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

