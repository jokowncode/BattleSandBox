using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

public class PassiveEntryWarehouseUI : WarehouseUI {
    
    public void UpdatePassiveEntryWarehouse(int passiveEntrySortCode) {
        ClearWarehouse();
        Dictionary<PassiveEntry, int> ownedPassiveEntries = PassiveEntryWarehouseManager.Instance.GetPassiveEntryFilterBySort(passiveEntrySortCode);
        foreach (var passiveEntryPair in ownedPassiveEntries){
            AddItem(passiveEntryPair.Key, passiveEntryPair.Value);
        }
    }

    private void AddItem(PassiveEntry passiveEntry, int count) {
        ClickableUI go = Instantiate(warehouseImageUIPrefab, warehouseContent);
        go.passiveEntryData = passiveEntry;
        go.UpdatePassiveEntryCount(count);
    }

    public void RecallPassiveEntry(PassiveEntry entry, int count = 1) {
        foreach (Transform child in warehouseContent) {
            if (child.TryGetComponent(out ClickableUI ui) && ui.passiveEntryData == entry) {
                ui.UpdatePassiveEntryCount(ui.passiveEntryCount + count);
                return;
            }
        }
        this.AddItem(entry, count);
    }
}
