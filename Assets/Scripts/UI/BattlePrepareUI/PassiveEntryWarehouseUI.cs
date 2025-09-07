using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        // TODO: Optimise Add And Remove PassiveEntry 
        PassiveEntry addEntry = passiveEntry; 
        if (isNewItem) {
            addEntry = Instantiate(passiveEntry, this.transform);
        }
        ClickableUI go = Instantiate(warehouseImageUIPrefab, warehouseContent);
        go.passiveEntryData = addEntry;
        if(isNewItem) PassiveEntryWarehouseManager.Instance.AddPassiveEntry(addEntry);
    }
}
