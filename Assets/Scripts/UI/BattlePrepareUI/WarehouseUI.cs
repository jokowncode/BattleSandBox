using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class WarehouseUI : MonoBehaviour {
    
    [SerializeField] protected GameObject warehouseImageUIPrefab;
    [SerializeField] protected Transform warehouseContent;           // ScrollView 的 Content 对象
    
    protected void ClearWarehouse(){
        foreach (Transform child in warehouseContent){
            Destroy(child.gameObject);
        }
    }

    public virtual void AddItem(Sprite sprite){
        GameObject go = Instantiate(warehouseImageUIPrefab, warehouseContent);
        Image imageComponent = go.GetComponent<Image>();
        if (imageComponent == null){
            imageComponent.AddComponent<Image>();
        }
        imageComponent.sprite = sprite;
    }

    public virtual void AddItem(PassiveEntry passiveEntry){ }
}
