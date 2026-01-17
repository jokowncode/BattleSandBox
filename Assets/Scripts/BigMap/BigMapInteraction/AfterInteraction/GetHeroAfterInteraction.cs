
using System.Collections.Generic;
using UnityEngine;

public class GetHeroAfterInteraction : MonoBehaviour {

    [SerializeField] private List<string> GetHeroNames;
    
    private void Awake() {
        if (this.TryGetComponent(out InteractionObject io)) {
            io.OnInteractionEnded += () => {
                foreach (string heroName in GetHeroNames) {
                    HeroWarehouseManager.Instance.AddHero(heroName);
                }
            };
        }
    }
}




