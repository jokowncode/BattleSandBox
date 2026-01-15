
using UnityEngine;

public class BattleDefeatGetHero : MonoBehaviour {

    [SerializeField] private string[] GetHeroNames;
    
    private void Awake() {
        if (TryGetComponent(out BattleRoom battleRoom)) {
            battleRoom.OnDefeat += () => {
                foreach (string heroName in GetHeroNames) {
                    HeroWarehouseManager.Instance.AddHero(heroName);
                }
            };
        }
    }
}


