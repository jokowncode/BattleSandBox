
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClueWarehouseManager : MonoBehaviour {

    [SerializeField] private List<ClueData> AllClueData;

    private List<string> OwnedClues;
    private Dictionary<string, ClueData> ClueMap;
    public static ClueWarehouseManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        this.ClueMap = new Dictionary<string, ClueData>();
        foreach (ClueData data in this.AllClueData) {
            this.ClueMap.Add(data.ClueName, data);
        }
    }
    
    private void Start() {
        SaveDataManager.Instance.OnLoadData += () => {
            this.OwnedClues = SaveDataManager.Instance.PlayerData.OwnedClues;
        };
    }

    public void AddClue(string clueName) {
        if (!this.ClueMap.ContainsKey(clueName)) return;
        if (this.OwnedClues.Contains(clueName)) return;
        this.OwnedClues.Add(clueName);
    }

    public List<ClueData> GetOwnedCluesByType(ClueType type) {
        List<ClueData> result = new List<ClueData>();
        foreach (string clueName in this.OwnedClues) {
            if (!this.ClueMap.ContainsKey(clueName)) continue;
            if (this.ClueMap[clueName].Type == type) {
                result.Add(this.ClueMap[clueName]);
            }
        }
        return result;
    }
}



