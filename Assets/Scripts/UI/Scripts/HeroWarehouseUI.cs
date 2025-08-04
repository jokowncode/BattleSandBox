using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class HeroWarehouseUI : MonoBehaviour {

    public static HeroWarehouseUI Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    public GameObject heroWarehouseWarriorUIPrefab;
    public GameObject heroWarehouseMageUIPrefab;
    public GameObject heroWarehousePriestUIPrefab;
    public GameObject defaultDraggableInstancePrefab;
    public Transform heroWarehouseContent;           // ScrollView 的 Content 对象
    

    public void UpdateHeroWarehouse() {
        
        ClearWarehouse();
        
        // List<string> ownedHeroes = HeroWarehouseManager.Instance.GetOwnedHeroes();
        // foreach (string heroRef in ownedHeroes) {
        //     Hero hero = AssetManager.Instance.LoadAsset<Hero>(heroRef);
        //     // TODO: Update Hero Warehouse UI
        //     
        // }
        //Debug.Log(HeroWarehouseManager.Instance);
        // List<string> ownedHeroes = HeroWarehouseManager.Instance.GetOwnedHeroes();
        // foreach (string hero in ownedHeroes)
        // {
        //     // TODO: Update Hero Warehouse UI
        //     AddItem(hero);
        // }
        // List<GameObject> ownedHeroes = HeroWarehouseManager.Instance.GetOwnedHeroes();
        // foreach (GameObject hero in ownedHeroes)
        // {
        //     // TODO: Update Hero Warehouse UI
        //     AddItem(hero.GetComponentInChildren<SpriteRenderer>().sprite,hero);
        // }
        List<string> ownedHeroes = HeroWarehouseManager.Instance.GetOwnedHeroesRef();
        foreach (string heroRef in ownedHeroes)
        {
            // TODO: Update Hero Warehouse UI
            AddItem(HeroWarehouseManager.Instance.GetHeroSpriteByRef(heroRef),heroRef);
        }
    }
    
    public void ClearWarehouse()
    {
        foreach (Transform child in heroWarehouseContent)
        {
            Destroy(child.gameObject);
        }
    }

    [Obsolete("use reference instead")]
    public void AddMageItem(Sprite sprite,string heroRef)
    {
        GameObject go = Instantiate(heroWarehouseMageUIPrefab, heroWarehouseContent);
        go.GetComponent<DraggableUI>().prefabReference = heroRef;
        Image imageComponent = go.GetComponent<Image>();
        if (imageComponent == null)
        {
            imageComponent.AddComponent<Image>();
        }
        imageComponent.sprite = sprite;
    }
    
    public void AddWarriorItem(Sprite sprite,string heroRef)
    {
        GameObject go = Instantiate(heroWarehouseWarriorUIPrefab, heroWarehouseContent);
        go.GetComponent<DraggableUI>().prefabReference = heroRef;
        Image imageComponent = go.GetComponent<Image>();
        if (imageComponent == null)
        {
            imageComponent.AddComponent<Image>();
        }
        imageComponent.sprite = sprite;
    }
    
    public void AddPriestItem(Sprite sprite,string heroRef)
    {
        GameObject go = Instantiate(heroWarehousePriestUIPrefab, heroWarehouseContent);
        go.GetComponent<DraggableUI>().prefabReference = heroRef;
        Image imageComponent = go.GetComponent<Image>();
        if (imageComponent == null)
        {
            imageComponent.AddComponent<Image>();
        }
        imageComponent.sprite = sprite;
    }
    
    public void AddItem(Sprite sprite,string heroRef)
    {
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
        // if (imageComponent == null)
        // {
        //     imageComponent.AddComponent<Image>();
        // }
        imageComponent[1].sprite = sprite;
    }
    
    // public void AddItem(Sprite sprite,string reference)
    // {
    //     GameObject go = Instantiate(heroWarehouseImageUIPrefab, heroWarehouseContent);
    //     go.GetComponent<DraggableUI>().prefabReference = reference;
    //     Image imageComponent = go.GetComponent<Image>();
    //     if (imageComponent == null)
    //     {
    //         imageComponent.AddComponent<Image>();
    //     }
    //     imageComponent.sprite = sprite;
    // }
    //
    // // test
    // public void AddItem(Sprite sprite)
    // {
    //     GameObject go = Instantiate(heroWarehouseImageUIPrefab, heroWarehouseContent);
    //     go.GetComponent<DraggableUI>().previewPrefab = defaultDraggableInstancePrefab;
    //     Image imageComponent = go.GetComponent<Image>();
    //     if (imageComponent == null)
    //     {
    //         imageComponent.AddComponent<Image>();
    //     }
    //     imageComponent.sprite = sprite;
    // }
    
    // public void AddItem(string hero)
    // {
    //     GameObject go = Instantiate(heroWarehouseImageUIPrefab, heroWarehouseContent);
    //     go.GetComponent<DraggableUI>().previewPrefab = defaultDraggableInstancePrefab;
    //     Image imageComponent = go.GetComponent<Image>();
    //     if (imageComponent == null)
    //     {
    //         imageComponent.AddComponent<Image>();
    //     }
    //     imageComponent.sprite = sprite;
    // }
    
    
    //-----------------------Debug-------------------------

    
}
