using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class HeroWarehouseUI : MonoBehaviour {
    
    public GameObject heroWarehouseWarriorUIPrefab;
    public GameObject heroWarehouseMageUIPrefab;
    public GameObject heroWarehousePriestUIPrefab;
    public GameObject defaultDraggableInstancePrefab;
    public Transform heroWarehouseContent;           // ScrollView 的 Content 对象


    public void Hide(){
        this.gameObject.SetActive(false);
    }

    public void UpdateHeroWarehouse() {
        ClearWarehouse();
        List<string> ownedHeroes = HeroWarehouseManager.Instance.GetOwnedHeroesRef();
        foreach (string heroRef in ownedHeroes){
            AddItem(HeroWarehouseManager.Instance.GetHeroSpriteByRef(heroRef),heroRef);
        }
    }
    
    private void ClearWarehouse(){
        foreach (Transform child in heroWarehouseContent){
            Destroy(child.gameObject);
        }
    }
    
    public void AddItem(Sprite sprite,string heroRef){
        GameObject go;
        FighterType tempType = HeroWarehouseManager.Instance.GetHeroType(heroRef);
        if(tempType == FighterType.Warrior)
            go = Instantiate(heroWarehouseWarriorUIPrefab, heroWarehouseContent);
        else if (tempType == FighterType.Mage)
            go = Instantiate(heroWarehouseMageUIPrefab, heroWarehouseContent);
        else
            go = Instantiate(heroWarehousePriestUIPrefab, heroWarehouseContent);
        go.GetComponent<DraggableUI>().prefabReference = heroRef;
        Image[] imageComponent = go.GetComponentsInChildren<Image>();
        imageComponent[1].sprite = sprite;
    }
}
