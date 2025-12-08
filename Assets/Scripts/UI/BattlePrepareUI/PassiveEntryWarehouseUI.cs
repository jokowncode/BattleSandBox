using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

public class PassiveEntryWarehouseUI : WarehouseUI {
    
    public void UpdatePassiveEntryWarehouse(int passiveEntrySortCode) {
        ClearWarehouse();
        List<PassiveEntry> ownedPassiveEntries = PassiveEntryWarehouseManager.Instance.GetPassiveEntryFilterBySort(passiveEntrySortCode);
        foreach (PassiveEntry passiveEntryData in ownedPassiveEntries){
            AddItem(passiveEntryData, false);
        }
    }

    public void AddItem(PassiveEntry passiveEntry, bool isNewItem) {
        ClickableUI go = Instantiate(warehouseImageUIPrefab, warehouseContent);
        go.passiveEntryData = passiveEntry;
        if (isNewItem) {
            PassiveEntryWarehouseManager.Instance.AddPassiveEntry(passiveEntry.Data.Name);
        }
    }
}
