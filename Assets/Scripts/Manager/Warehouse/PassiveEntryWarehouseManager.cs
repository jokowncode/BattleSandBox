using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEntryWarehouseManager : MonoBehaviour {

    [SerializeField] private List<PassiveEntry> AllPassiveEntries;
    
    private List<string> OwnedPassiveEntries = new List<string>();
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
        
        // TODO: TEMP Debug Battle
        foreach (PassiveEntry entry in AllPassiveEntries) {
            this.OwnedPassiveEntries.Add(entry.Data.Name);
        }
    }

    private void Start() {
        // TODO: TEMP Debug Battle
        /*if (PlayerPrefs.HasKey("OwnedPassiveEntryWarehouse")) {
            string json = PlayerPrefs.GetString("OwnedPassiveEntryWarehouse");
            this.OwnedPassiveEntries = JsonUtility.FromJson<Serialization<string>>(json).ToList();
        }*/
    }

    private void OnDestroy() {
        // TODO: TEMP Debug Battle
        /*string json = JsonUtility.ToJson(new Serialization<string>(this.OwnedPassiveEntries));
        PlayerPrefs.SetString("OwnedPassiveEntryWarehouse", json);*/
    }

    public List<PassiveEntry> GetPassiveEntryFilterBySort(int sortCode) {
        List<PassiveEntry> result = new List<PassiveEntry>();
        foreach (string passiveEntryName in this.OwnedPassiveEntries) {
            if (this.AllPassiveEntryMap.TryGetValue(passiveEntryName, out PassiveEntry entry)) {
                if ((entry.GetSortCode() & sortCode) == 0) continue;
                result.Add(entry);    
            }
        }
        return result;
    }

    public void AddPassiveEntry(string passiveEntry) {
        this.OwnedPassiveEntries.Add(passiveEntry);
    }

    public void RemovePassiveEntry(string passiveEntry) {
        if (ContainsPassiveEntry(passiveEntry)) {
            this.OwnedPassiveEntries.Remove(passiveEntry);
        }
    }

    public bool ContainsPassiveEntry(string passiveEntry) {
        return this.OwnedPassiveEntries.Contains(passiveEntry);
    }

    public PassiveEntry GetPassiveEntryByName(string passiveEntryName) {
        // if (!ContainsPassiveEntry(passiveEntryName)) return null;
        return this.AllPassiveEntryMap.GetValueOrDefault(passiveEntryName);
    }
    
}
