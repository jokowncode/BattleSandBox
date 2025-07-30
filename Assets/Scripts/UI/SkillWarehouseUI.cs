using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class SkillWarehouseUI : MonoBehaviour
{
    public static SkillWarehouseUI Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    
    public GameObject skillWarehouseImageUIPrefab;
    public Transform skillWarehouseContent;           // ScrollView 的 Content 对象

    [Header("Debugger")]
    public bool isDebugging;
    
    [Header("Test Sprite")]
    public Sprite testSprite1;
    public Sprite testSprite2;
    public Sprite testSprite3;

    public void Update()
    {
        MyDebug();
    }

    public void UpdateHeroWarehouse() {
        
        ClearWarehouse();
        
        // List<string> ownedHeroes = HeroWarehouseManager.Instance.GetOwnedHeroes();
        // foreach (string heroRef in ownedHeroes) {
        //     Hero hero = AssetManager.Instance.LoadAsset<Hero>(heroRef);
        //     // TODO: Update Hero Warehouse UI
        //     
        // }
    }
    
    public void ClearWarehouse()
    {
        foreach (Transform child in skillWarehouseContent)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddItem(Sprite sprite)
    {
        GameObject go = Instantiate(skillWarehouseImageUIPrefab, skillWarehouseContent);
        Image imageComponent = go.GetComponent<Image>();
        if (imageComponent == null)
        {
            imageComponent.AddComponent<Image>();
        }
        imageComponent.sprite = sprite;
    }
    
    
    //-----------------------Debug-------------------------
    public void MyDebug()
    {
        if (!isDebugging) return;
        
        DebugClearHeroWarehouse();
        DebugAddItem();
    }

    public void DebugClearHeroWarehouse()
    {
        if (Input.GetKeyDown((KeyCode.C)))
        {
            Debug.Log("ExecuteClearHeroWarehouse");
            ClearWarehouse();
        }
    }

    public void DebugAddItem()
    {
        if (Input.GetKeyDown((KeyCode.A)))
        {
            Debug.Log("ExecuteAddItem");
            AddItem(testSprite2);
            AddItem(testSprite1);
            AddItem(testSprite3);
        }
    }
    
}
