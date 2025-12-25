
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveMapManager : MonoBehaviour {

    public static SaveMapManager Instance;
    
    // public Action OnLoadMap;
    
    public Player PlayerInBigMap { get; private set; }

    private PlayerBigMapSaveData BigMapPlayerData;
    private Dictionary<string, bool> InteractionObjectsEndMap;
    private List<string> AvailableDialogues;

    private bool IsEnterBigMap = false;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        // this.LoadData();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadData() {
        if (PlayerPrefs.HasKey("PlayerBigMapData")) {
            this.BigMapPlayerData = JsonUtility.FromJson<PlayerBigMapSaveData>(PlayerPrefs.GetString("PlayerBigMapData"));
        } else {
            this.BigMapPlayerData = new PlayerBigMapSaveData();
        }

        if (PlayerPrefs.HasKey("InteractionObjectEnd")) {
            this.InteractionObjectsEndMap = JsonUtility.FromJson<Serialization<string, bool>>(PlayerPrefs.GetString("InteractionObjectEnd"))
                .ToDictionary();
        } else {
            this.InteractionObjectsEndMap = new Dictionary<string, bool>();
        }
        
        if (PlayerPrefs.HasKey("AvailableDialogues")) {
            this.AvailableDialogues = JsonUtility.FromJson<Serialization<string>>(PlayerPrefs.GetString("AvailableDialogues")).ToList();
        } else {
            this.AvailableDialogues = new List<string>();
        }
    }

    private void Start() {
        SceneChangeManager.Instance.OnSceneChange += OnSceneChange;
    }

    private void OnSceneChange(SceneType oldScene, SceneType newScene) {
        if (oldScene == SceneType.BigMap) {
            this.BigMapPlayerData.PlayerPosition = this.PlayerInBigMap.transform.position;
            if (newScene != SceneType.Battle) {
                this.OnDisable();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneChangeManager.Instance.CurrentScene == SceneType.BigMap) {
            this.PlayerInBigMap = FindAnyObjectByType<Player>();
            // this.OnLoadMap?.Invoke();
            if (this.BigMapPlayerData.PlayerPosition != Vector3.zero) {
                this.PlayerInBigMap.transform.position = this.BigMapPlayerData.PlayerPosition;
            }
            IsEnterBigMap = true;
        }
    }

    private void OnDisable() {
        if (!IsEnterBigMap) return;
        if (this.PlayerInBigMap) {
            this.BigMapPlayerData.PlayerPosition = this.PlayerInBigMap.transform.position;
        }
        string dataJson = JsonUtility.ToJson(this.BigMapPlayerData);
        PlayerPrefs.SetString("PlayerBigMapData", dataJson);
        
        string interactionJson = JsonUtility.ToJson(new Serialization<string, bool>(this.InteractionObjectsEndMap));
        PlayerPrefs.SetString("InteractionObjectEnd", interactionJson);
        
        string dialoguesJson = JsonUtility.ToJson(new Serialization<string>(this.AvailableDialogues));
        PlayerPrefs.SetString("AvailableDialogues", dialoguesJson);
    }

    public void SaveInteractionObject(string objName, bool isEnd) {
        if (!this.InteractionObjectsEndMap.TryAdd(objName, isEnd)) {
            this.InteractionObjectsEndMap[objName] = isEnd;
        }
    }

    public bool LoadInteractionObject(string objName) {
        return this.InteractionObjectsEndMap.GetValueOrDefault(objName, false);
    }

    public bool DialoguesAvailable(string dialogueName) {
        return this.AvailableDialogues.Contains(dialogueName);
    }

    public void SaveAvailableDialogue(string dialogueName) {
        if (!this.AvailableDialogues.Contains(dialogueName)) {
            this.AvailableDialogues.Add(dialogueName);
        }
    }

    public int CurrentTaskIndex {
        get => this.BigMapPlayerData.CurrentTaskIndex;
        set => this.BigMapPlayerData.CurrentTaskIndex = value;
    }
}


