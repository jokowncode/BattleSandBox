
using UnityEngine;

public class VictoryState : BattleState{
    
    public override void Construct() {
        BattleUIManager.Instance.GameEnd(true);
        Controller.BattleVictoryAddHeroBond();
        
#if DEBUG_MODE
        float duration = Time.time - Controller.BattleStartTime;
        Debug.Log($"Battle Duration : {duration}");
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle){
            Debug.Log($"{hero.gameObject.name} Survive -> Caused Total Damage: {hero.TotalDamage}, DPS: {hero.TotalDamage / duration}");
        }
#endif
    }
}

