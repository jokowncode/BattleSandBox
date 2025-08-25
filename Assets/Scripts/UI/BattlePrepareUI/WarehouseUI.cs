using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class WarehouseUI : MonoBehaviour {
    
    [SerializeField] protected ClickableUI warehouseImageUIPrefab;
    [SerializeField] protected Transform warehouseContent;           // ScrollView 的 Content 对象
    
    protected void ClearWarehouse(){
        foreach (Transform child in warehouseContent){
            Destroy(child.gameObject);
        }
    } 
}
