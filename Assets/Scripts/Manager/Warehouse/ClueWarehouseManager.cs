
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClueWarehouseManager : MonoBehaviour {

    [SerializeField] private List<ClueData> AllClueData;

    [Header("UI")] 
    [SerializeField] private ClueListPanel ClueListPanelUI;
    [SerializeField] private ClueDetailPanel ClueDetailPanelUI;
    [field: SerializeField] public Sprite[] ClueIcons { get; private set; }

    private List<string> OwnedClues;
    private Dictionary<string, ClueData> ClueMap;
    public static ClueWarehouseManager Instance;

    private CanvasGroup ClueCanvasGroup;
    
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
        this.ClueCanvasGroup = this.GetComponent<CanvasGroup>();
        this.ClueListPanelUI.OnClueClicked += OnClueClicked;
    }

    private void OnClueClicked(string cName) {
        this.ClueDetailPanelUI.SetClue(cName);
    }

    public void TransitionShowUI(bool show) {
        if (show && this.ClueCanvasGroup.alpha >= 0.9f) return;
        this.ClueCanvasGroup.alpha = show ? 1.0f : 0.0f;
        this.ClueCanvasGroup.blocksRaycasts = show;
        this.ClueCanvasGroup.interactable = show;
        if (show) {
            this.ClueListPanelUI.Show();
        } else {
            this.ClueDetailPanelUI.GoBackToNormal();
        }
    }

    private void Start() {
        SaveDataManager.Instance.OnLoadData += () => {
            this.OwnedClues = SaveDataManager.Instance.PlayerData.OwnedClues;
            
            // TODO: TEMP
            if (this.OwnedClues.Count == 0) {
                foreach (ClueData clue in this.AllClueData) {
                    AddClue(clue.ClueName);
                }
            }
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

    public ClueData GetClueByName(string clueName) {
        return this.ClueMap.GetValueOrDefault(clueName);
    }
}



