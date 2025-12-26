
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveMapManager : MonoBehaviour {

    public static SaveMapManager Instance;
    
    // public Action OnLoadMap;
    
    public Player PlayerInBigMap { get; private set; }

    private PlayerBigMapSaveData BigMapPlayerData;
    private List<string> InteractionObjectsEnds;
    private List<string> InteractionObjectsAvailable;
    private Dictionary<string, float> DungeonHeroHealth;

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
            this.InteractionObjectsEnds = JsonUtility.FromJson<Serialization<string>>(PlayerPrefs.GetString("InteractionObjectEnd"))
                .ToList();
        } else {
            this.InteractionObjectsEnds = new List<string>();
        }
        
        if (PlayerPrefs.HasKey("InteractionObjectsAvailable")) {
            this.InteractionObjectsAvailable = JsonUtility.FromJson<Serialization<string>>(PlayerPrefs.GetString("InteractionObjectsAvailable")).ToList();
        } else {
            this.InteractionObjectsAvailable = new List<string>();
        }
        
        if (PlayerPrefs.HasKey("DungeonHeroHealth")) {
            this.DungeonHeroHealth = JsonUtility.FromJson<Serialization<string, float>>(PlayerPrefs.GetString("DungeonHeroHealth"))
                .ToDictionary();
        } else {
            this.DungeonHeroHealth = new Dictionary<string, float>();
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
        
        string interactionJson = JsonUtility.ToJson(new Serialization<string>(this.InteractionObjectsEnds));
        PlayerPrefs.SetString("InteractionObjectEnd", interactionJson);
        
        string dialoguesJson = JsonUtility.ToJson(new Serialization<string>(this.InteractionObjectsAvailable));
        PlayerPrefs.SetString("InteractionObjectsAvailable", dialoguesJson);
        
        string dungeonHeroHealthJson = JsonUtility.ToJson(new Serialization<string, float>(this.DungeonHeroHealth));
        PlayerPrefs.SetString("DungeonHeroHealth", dungeonHeroHealthJson);
    }
    
    public void ClearDungeonData() {
        PlayerPrefs.DeleteKey("PlayerBigMapData");
        PlayerPrefs.DeleteKey("InteractionObjectEnd");
        PlayerPrefs.DeleteKey("InteractionObjectsAvailable");
        PlayerPrefs.DeleteKey("DungeonHeroHealth");
    }

    public void SetInteractionObjectEnd(string objName) {
        if (!this.InteractionObjectsEnds.Contains(objName)) {
            this.InteractionObjectsEnds.Add(objName);
        }
    }

    public bool LoadInteractionObjectEnd(string objName) {
        return this.InteractionObjectsEnds.Contains(objName);
    }

    public bool LoadInteractionObjectAvailable(string dialogueName) {
        return this.InteractionObjectsAvailable.Contains(dialogueName);
    }

    public void SetInteractionObjectAvailable(string dialogueName) {
        if (!this.InteractionObjectsAvailable.Contains(dialogueName)) {
            this.InteractionObjectsAvailable.Add(dialogueName);
        }
    }

    public float GetHeroHealth(string heroName) {
        return this.DungeonHeroHealth.GetValueOrDefault(heroName, -1.0f);
    }

    public void SetHeroHealth(string heroName, float health) {
        if (!this.DungeonHeroHealth.TryAdd(heroName, health)) {
            this.DungeonHeroHealth[heroName] = health;
        }
    }

    public void RecoverAllHeroHealth() {
        this.DungeonHeroHealth.Clear();
    }

    public int CurrentTaskIndex {
        get => this.BigMapPlayerData.CurrentTaskIndex;
        set => this.BigMapPlayerData.CurrentTaskIndex = value;
    }
}


