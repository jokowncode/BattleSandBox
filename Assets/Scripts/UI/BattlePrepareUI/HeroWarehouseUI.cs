using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class HeroWarehouseUI : MonoBehaviour {
    
    public HeroPanelUI heroWarehouseWarriorUIPrefab;
    public HeroPanelUI heroWarehouseMageUIPrefab;
    public HeroPanelUI heroWarehousePriestUIPrefab;
    public Transform heroWarehouseContent;           // ScrollView 的 Content 对象


    public void Hide(){
        this.gameObject.SetActive(false);
    }

    public void UpdateHeroWarehouse() {
        ClearWarehouse();
        List<string> ownedHeroes = HeroWarehouseManager.Instance.GetOwnedHeroesRef();
        foreach (string heroRef in ownedHeroes){
            AddItem(heroRef);
        }
    }
    
    private void ClearWarehouse(){
        foreach (Transform child in heroWarehouseContent){
            Destroy(child.gameObject);
        }
    }
    
    public void AddItem(string heroRef){
        HeroPanelUI go;
        Hero hero = HeroWarehouseManager.Instance.GetHeroByRef(heroRef);
        if(hero.Type == FighterType.Warrior)
            go = Instantiate(heroWarehouseWarriorUIPrefab, heroWarehouseContent);
        else if (hero.Type == FighterType.Mage)
            go = Instantiate(heroWarehouseMageUIPrefab, heroWarehouseContent);
        else
            go = Instantiate(heroWarehousePriestUIPrefab, heroWarehouseContent);
        
        go.SetPortrait(hero.WarehouseHeroPortrait, false);
        float health = SaveDataManager.Instance.GetHeroHealth(hero.Name);
        if (health == 0.0f) {
            go.HeroDead();
            return;
        }

        DraggableUI draggableUI = go.AddComponent<DraggableUI>();
        draggableUI.prefabReference = heroRef;
    }
}
