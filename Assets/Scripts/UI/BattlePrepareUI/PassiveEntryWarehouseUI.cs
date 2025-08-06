using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class PassiveEntryWarehouseUI : WarehouseUI {
    
    public void UpdateHeroWarehouse() {
        ClearWarehouse();
        List<PassiveEntry> ownedHeroes = PassiveEntryWarehouseManager.Instance.GetOwnedHeroes();
        foreach (PassiveEntry skillData in ownedHeroes){
            AddItem(skillData);
        }
    }

    public override void AddItem(PassiveEntry passiveEntry){
        GameObject go = Instantiate(warehouseImageUIPrefab, warehouseContent);
        go.GetComponentInChildren<ClickableUI>().passiveEntryData = passiveEntry;
    }
}
