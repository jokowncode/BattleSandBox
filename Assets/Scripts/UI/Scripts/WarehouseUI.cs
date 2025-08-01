using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class WarehouseUI : MonoBehaviour
{
    
    public GameObject warehouseImageUIPrefab;
    public Transform warehouseContent;           // ScrollView 的 Content 对象
    
    [Header("Debugger")]
    public bool isDebugging;
    
    [Header("Test Sprite")]
    public Sprite testSprite1;
    public Sprite testSprite2;
    public Sprite testSprite3;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ClearWarehouse()
    {
        foreach (Transform child in warehouseContent)
        {
            Destroy(child.gameObject);
        }
    }

    public virtual void AddItem(Sprite sprite)
    {
        GameObject go = Instantiate(warehouseImageUIPrefab, warehouseContent);
        Image imageComponent = go.GetComponent<Image>();
        if (imageComponent == null)
        {
            imageComponent.AddComponent<Image>();
        }
        imageComponent.sprite = sprite;
    }

    public virtual void AddItem(SkillData skillData)
    {
        return;
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
