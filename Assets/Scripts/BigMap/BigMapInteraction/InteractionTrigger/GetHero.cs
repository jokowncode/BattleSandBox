
using System.Collections.Generic;
using UnityEngine;

public class GetHero : InteractionTrigger {

    [ScriptableObjectNameProp(typeof(FighterData), "Name")]
    [SerializeField] private List<string> GetHeroNames;
    
    protected override void TriggerAction() {
        foreach (string heroName in GetHeroNames) {
            HeroWarehouseManager.Instance.AddHero(heroName);
        }
    }
}




