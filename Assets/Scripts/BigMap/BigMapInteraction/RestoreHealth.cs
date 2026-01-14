
using System.Collections.Generic;
using UnityEngine;

public class RestoreHealth : InteractionObject {

    [SerializeField] private bool CanReviveHero = false;
    [SerializeField] private float RestoreHealthPercentage = 0.2f;
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActiveWhenAwake = true;
        base.Awake();
    }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.回血;
    }

    protected override void Interaction() {
        List<string> heroes = HeroWarehouseManager.Instance.GetOwnedHeroesRef();
        foreach (string heroName in heroes) {
            float currentHealth = SaveDataManager.Instance.GetHeroHealth(heroName);
            if (currentHealth < 0.0f) continue;
            if(!CanReviveHero && currentHealth == 0.0f) continue;
            Hero hero = HeroWarehouseManager.Instance.GetHeroByRef(heroName);
            if (hero) {
                currentHealth += hero.InitialHealth * this.RestoreHealthPercentage;
                SaveDataManager.Instance.SetHeroHealth(heroName, currentHealth);
            }
        }
    }
}



