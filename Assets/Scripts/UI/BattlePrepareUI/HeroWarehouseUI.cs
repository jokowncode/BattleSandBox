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
        FighterType tempType = HeroWarehouseManager.Instance.GetHeroType(heroRef);
        if(tempType == FighterType.Warrior)
            go = Instantiate(heroWarehouseWarriorUIPrefab, heroWarehouseContent);
        else if (tempType == FighterType.Mage)
            go = Instantiate(heroWarehouseMageUIPrefab, heroWarehouseContent);
        else
            go = Instantiate(heroWarehousePriestUIPrefab, heroWarehouseContent);
        if(go.GetComponent<DraggableUI>()==null)
            go.AddComponent<DraggableUI>();
        go.GetComponent<DraggableUI>().prefabReference = heroRef;
        go.SetPortrait(HeroWarehouseManager.Instance.GetHeroByRef(heroRef).heroPortraitSprite);
    }
}
