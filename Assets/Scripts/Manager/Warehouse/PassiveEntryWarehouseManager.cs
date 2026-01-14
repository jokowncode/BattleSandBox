using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEntryWarehouseManager : MonoBehaviour {

    [SerializeField] private List<PassiveEntry> AllPassiveEntries;
    
    // private List<string> OwnedPassiveEntries = new List<string>();
    
    private SerializableDictionary<string, int> OwnedPassiveEntries;
    
    public static PassiveEntryWarehouseManager Instance;

    private Dictionary<string, PassiveEntry> AllPassiveEntryMap;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        this.AllPassiveEntryMap = new Dictionary<string, PassiveEntry>();
        foreach (PassiveEntry entry in AllPassiveEntries) {
            this.AllPassiveEntryMap.Add(entry.Data.Name, entry);
        }
    }

#if TEST_BATTLE
    public void TEMPFORBATTLE() {
        if (this.OwnedPassiveEntries == null || this.OwnedPassiveEntries.Count == 0) {
            this.OwnedPassiveEntries = new SerializableDictionary<string, int>();
            foreach (PassiveEntry entry in AllPassiveEntries) {
                this.OwnedPassiveEntries.Add(entry.Data.Name, 3);
            }    
        }
    }
#endif
    
    private void Start() {
        SaveDataManager.Instance.OnLoadData += () => {
            // TODO: TEMP Debug Battle
            this.OwnedPassiveEntries = SaveDataManager.Instance.PlayerData.OwnedPassiveEntries;
            if (this.OwnedPassiveEntries.Count == 0) {
                foreach (PassiveEntry entry in AllPassiveEntries) {
                    this.OwnedPassiveEntries.Add(entry.Data.Name, 3);
                }    
            }
        };
    }

    public Dictionary<PassiveEntry, int> GetPassiveEntryFilterBySort(int sortCode) {
        Dictionary<PassiveEntry, int> result = new Dictionary<PassiveEntry, int>();
        foreach (KeyValuePair<string, int> passiveEntryPair in this.OwnedPassiveEntries) {
            if (this.AllPassiveEntryMap.TryGetValue(passiveEntryPair.Key, out PassiveEntry entry)) {
                if ((entry.GetSortCode() & sortCode) == 0) continue;
                result.Add(entry, passiveEntryPair.Value);    
            }
        }
        return result;
    }

    public void AddPassiveEntry(string passiveEntry, int count) {
        this.OwnedPassiveEntries.TryAdd(passiveEntry, 0);
        this.OwnedPassiveEntries[passiveEntry] += count;
    }

    public void RemovePassiveEntry(string passiveEntry) {
        if (ContainsPassiveEntry(passiveEntry)) {
            this.OwnedPassiveEntries[passiveEntry] -= 1;
            if (this.OwnedPassiveEntries[passiveEntry] <= 0) {
                this.OwnedPassiveEntries.Remove(passiveEntry);
            }
        }
    }

    public bool ContainsPassiveEntry(string passiveEntry) {
        return this.OwnedPassiveEntries.ContainsKey(passiveEntry);
    }

    public PassiveEntry GetPassiveEntryByName(string passiveEntryName) {
        // if (!ContainsPassiveEntry(passiveEntryName)) return null;
        return this.AllPassiveEntryMap.GetValueOrDefault(passiveEntryName);
    }

    public bool UpgradePassiveEntry(string entryName) {
        if (!ContainsPassiveEntry(entryName)) return false;
        if (this.OwnedPassiveEntries[entryName] < 3) return false;

        PassiveEntry entry = GetPassiveEntryByName(entryName);
        if (!entry.UpgradePassiveEntry) return false;

        this.OwnedPassiveEntries[entryName] -= 3;
        string upgradePassiveEntryName = entry.UpgradePassiveEntry.Data.Name;
        this.AddPassiveEntry(upgradePassiveEntryName, 1);
        return true;
    }
}
