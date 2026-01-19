
using System.Collections.Generic;
using UnityEngine;

public class RemoveHero : InteractionTrigger {
    
    [SerializeField] private List<string> GetHeroNames;
    
    protected override void TriggerAction() {
        foreach (string heroName in GetHeroNames) {
            HeroWarehouseManager.Instance.RemoveHero(heroName);
        }
    }
}


