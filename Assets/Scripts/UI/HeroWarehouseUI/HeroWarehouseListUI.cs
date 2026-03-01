
using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroWarehouseListUI : MonoBehaviour {
    
    [Header("Hero List")] 
    [SerializeField] private bool ClickFirst = true;
    [SerializeField] private Transform HeroListContainer;

    private HeroWarehouseCategory CurrentCategory = HeroWarehouseCategory.All;

    public Action<string> OnHeroClicked;

    private void Awake() {
        foreach (Transform child in this.HeroListContainer) {
            if (child.TryGetComponent(out HeroListSingle single)) {
                single.OnClicked += heroName => {
                    this.OnHeroClicked?.Invoke(heroName);
                };
            }
        }
    }

    public void WarriorCategory() {
        this.UpdateHeroList(HeroWarehouseCategory.Warrior);
    }
    
    public void MageCategory() {
        this.UpdateHeroList(HeroWarehouseCategory.Mage);
    }
    
    public void PriestCategory() {
        this.UpdateHeroList(HeroWarehouseCategory.Priest);
    }
    
    public void AllCategory(bool isFirst) {
        this.UpdateHeroList(HeroWarehouseCategory.All, isFirst);
    }

    public void UpdateHeroHealth() {
        foreach (Transform child in this.HeroListContainer) {
            if (child.TryGetComponent(out HeroListSingle single)) {
                single.UpdateHeroHealth();
            }
        }
    }

    private void UpdateHeroList(HeroWarehouseCategory category = HeroWarehouseCategory.All, bool isFirst = false) {
        if (this.CurrentCategory == category && !isFirst) return;
        CurrentCategory = category;
        List<Hero> ownedHeroList = HeroWarehouseManager.Instance.GetHeroesByType(category);
        for (int i = 0; i < this.HeroListContainer.childCount; i++) {
            Transform child = this.HeroListContainer.GetChild(i);
            if (child.TryGetComponent(out HeroListSingle single)) {
                if (i < ownedHeroList.Count) {
                    Sprite borderSprite = HeroWarehouseManager.Instance.GetHeroBorderSprite(ownedHeroList[i].Type);
                    single.SetContent(ownedHeroList[i], borderSprite, ownedHeroList[i].WarehouseData.AvatarSprite);
                    if (isFirst && i == 0 && this.ClickFirst) {
                        single.ClickButton();
                    }
                } else {
                    single.SetContent(null, HeroWarehouseManager.Instance.EmptyBorderSprite, null);
                }
            }
        }
    }
    
    
}

