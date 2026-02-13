
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
        if (this.RestoreHealthPercentage >= 1.0f && this.CanReviveHero) {
            SceneChangeManager.Instance.AddGameTip($"全员回满血");
            SaveDataManager.Instance.RecoverAllHeroHealth();
            return;
        }

        List<string> heroes = HeroWarehouseManager.Instance.GetOwnedHeroesRef();
        foreach (string heroName in heroes) {
            float currentHealth = SaveDataManager.Instance.GetHeroHealth(heroName);
            if (currentHealth < 0.0f) continue;
            if(!CanReviveHero && currentHealth == 0.0f) continue;
            Hero hero = HeroWarehouseManager.Instance.GetHeroByRef(heroName);
            if (hero) {
                if (currentHealth >= hero.InitialHealth) continue;
                string revive = currentHealth == 0.0f ? "已复活" : "回血后";
                currentHealth += hero.InitialHealth * this.RestoreHealthPercentage;
                currentHealth = Mathf.Min(currentHealth, hero.InitialHealth);
                SaveDataManager.Instance.SetHeroHealth(heroName, currentHealth);
                SceneChangeManager.Instance.AddGameTip($"{hero.WarehouseData.HeroChineseName}{revive}：{currentHealth}");
            }
        }
        SceneChangeManager.Instance.AddGameTip($"全员回血{this.RestoreHealthPercentage * 100}%");
    }
}



