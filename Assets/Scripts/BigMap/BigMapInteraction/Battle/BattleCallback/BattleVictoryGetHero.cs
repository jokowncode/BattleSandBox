
using UnityEngine;

public class BattleVictoryGetHero : MonoBehaviour {
    
    [SerializeField] private string[] GetHeroNames;
    
    private void Awake() {
        if (TryGetComponent(out BattleRoom battleRoom)) {
            battleRoom.OnVictory += () => {
                foreach (string heroName in GetHeroNames) {
                    HeroWarehouseManager.Instance.AddHero(heroName);
                }
            };
        }
    }
}

