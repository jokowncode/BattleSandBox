using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PassiveEntryWarehouseManager : MonoBehaviour {

    [field: SerializeField] public int SynthPassiveEntryRequiredCount { get; private set; } = 3;
    [SerializeField] private List<PassiveEntry> AllPassiveEntries;
    
    [Header("Synth")]
    [SerializeField] private PassiveEntrySynthParentPanel SynthPanel;
    
    // private List<string> OwnedPassiveEntries = new List<string>();
    
    private SerializableDictionary<string, int> OwnedPassiveEntries;
    
    public static PassiveEntryWarehouseManager Instance;

    private Dictionary<string, PassiveEntry> AllPassiveEntryMap;

    public bool HasPassiveEntry => this.OwnedPassiveEntries.Count != 0;
    
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
            this.OwnedPassiveEntries = SaveDataManager.Instance.PlayerData.OwnedPassiveEntries;
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
    
    public Dictionary<PassiveEntry, int> GetPassiveEntryByStar(int star) {
        Dictionary<PassiveEntry, int> result = new Dictionary<PassiveEntry, int>();
        foreach (KeyValuePair<string, int> passiveEntryPair in this.OwnedPassiveEntries) {
            if (this.AllPassiveEntryMap.TryGetValue(passiveEntryPair.Key, out PassiveEntry entry)) {
                if (entry.Data.Star != star) continue;
                result.Add(entry, passiveEntryPair.Value);    
            }
        }
        return result;
    }

    public PassiveEntryData GetRandomPassiveEntryByStar(int star) {
        List<PassiveEntry> entries = this.AllPassiveEntries.FindAll(entry => entry.Data.Star == star);
        if (entries.Count == 0) return null;
        int randomIndex = Random.Range(0, entries.Count);
        return entries[randomIndex].Data;
    }

    /*public Dictionary<string, int> GetRandomPassiveEntryDataByStar(int maxCount, int maxStar) {
        Dictionary<string, int> result = new();
        if (maxCount == 0) return result;
        int rest = maxCount;

        List<PassiveEntry> entries = this.AllPassiveEntries.FindAll(entry => entry.Data.Star <= maxStar);
        while (rest != 0) {
            int index = Random.Range(0, entries.Count);
            int count = Random.Range(1, rest);
            rest -= count;
            string key = entries[index].Data.Name;
            result.TryAdd(key, 0);
            result[key] += count;
        }
        return result;
    }*/

    public void OpenPassiveEntrySynthPanel() {
        this.SynthPanel.TransitionShow(true);
    }

    public void AddPassiveEntry(string passiveEntry, int count) {
        if (!this.AllPassiveEntryMap.ContainsKey(passiveEntry)) return;
        this.OwnedPassiveEntries.TryAdd(passiveEntry, 0);
        this.OwnedPassiveEntries[passiveEntry] += count;
    }

    public void AddRandomPassiveEntry(int star, int count) {
        PassiveEntryData data = GetRandomPassiveEntryByStar(star);
        if (!data) return;
        SceneChangeManager.Instance.AddGameTip($"获得芯片：{data.Name}");
        this.AddPassiveEntry(data.Name, count);
    }

    public void RemovePassiveEntry(string passiveEntry, int count = 1) {
        if (ContainsPassiveEntry(passiveEntry)) {
            this.OwnedPassiveEntries[passiveEntry] -= count;
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
        if (this.OwnedPassiveEntries[entryName] < this.SynthPassiveEntryRequiredCount) return false;

        PassiveEntry entry = GetPassiveEntryByName(entryName);
        if (!entry.UpgradePassiveEntry) return false;
        
        this.RemovePassiveEntry(entryName, this.SynthPassiveEntryRequiredCount);
        string upgradePassiveEntryName = entry.UpgradePassiveEntry.Data.Name;
        this.AddPassiveEntry(upgradePassiveEntryName, 1);
        return true;
    }
}
