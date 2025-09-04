using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEntryWarehouseManager : MonoBehaviour{

    [SerializeField] private List<PassiveEntry> OwnedPassiveEntries;
    public static PassiveEntryWarehouseManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public List<PassiveEntry> GetOwnedPassiveEntries() { return this.OwnedPassiveEntries; }

    public List<PassiveEntry> GetPassiveEntryFilterBySort(int sortCode) {
        List<PassiveEntry> result = new List<PassiveEntry>();
        foreach (PassiveEntry passiveEntry in this.OwnedPassiveEntries) {
            if ((passiveEntry.GetSortCode() & sortCode) == 0) continue;
            result.Add(passiveEntry);
        }
        return result;
    }

    public void AddPassiveEntry(PassiveEntry passiveEntry) {
        this.OwnedPassiveEntries.Add(passiveEntry);
    }

    public void RemovePassiveEntry(PassiveEntry passiveEntry) {
        if (ContainsPassiveEntry(passiveEntry)) {
            this.OwnedPassiveEntries.Remove(passiveEntry);
        }
    }

    public bool ContainsPassiveEntry(PassiveEntry passiveEntry) {
        return this.OwnedPassiveEntries.Contains(passiveEntry);
    }
    
}
