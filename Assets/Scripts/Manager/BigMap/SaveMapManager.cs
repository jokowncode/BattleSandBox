
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveMapManager : MonoBehaviour {

    public static SaveMapManager Instance;
    
    // public Action OnLoadMap;
    
    public Player PlayerInBigMap { get; private set; }

    private Dictionary<SceneType, Vector3> PlayerPos;
    private List<string> InteractionObjectsEnds;
    private List<string> InteractionObjectsAvailable;
    private Dictionary<string, float> DungeonHeroHealth;

    public Action OnSaveData;
    public Action OnLoadData;

    private Vector3 TempPlayerPos = Vector3.zero;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        
        // TODO: TEMP -> CONVENIENT BATTLE TEST
        this.DungeonHeroHealth = new Dictionary<string, float>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start() {
        SceneChangeManager.Instance.OnSceneChange += OnSceneChange;
    }

    private void OnSceneChange(SceneType oldScene, SceneType newScene) {
        if (oldScene == SceneType.BigMap && SceneTools.IsBattleScene(newScene)) {
            if(this.PlayerInBigMap) this.TempPlayerPos = this.PlayerInBigMap.transform.position;
        }
    }

    public void SaveData() {
        if (this.PlayerInBigMap) {
            SceneType currentDungeon = SceneChangeManager.Instance.DungeonScene;
            this.PlayerPos[currentDungeon] = this.PlayerInBigMap.transform.position;
        }
        
        string dataJson = JsonUtility.ToJson(new Serialization<SceneType, Vector3>(this.PlayerPos));
        PlayerPrefs.SetString("PlayerPos", dataJson);
        
        string interactionJson = JsonUtility.ToJson(new Serialization<string>(this.InteractionObjectsEnds));
        PlayerPrefs.SetString("InteractionObjectEnd", interactionJson);
        
        string dialoguesJson = JsonUtility.ToJson(new Serialization<string>(this.InteractionObjectsAvailable));
        PlayerPrefs.SetString("InteractionObjectsAvailable", dialoguesJson);
        
        string dungeonHeroHealthJson = JsonUtility.ToJson(new Serialization<string, float>(this.DungeonHeroHealth));
        PlayerPrefs.SetString("DungeonHeroHealth", dungeonHeroHealthJson);
        this.OnSaveData?.Invoke();
    }

    public void LoadData() {
        if (PlayerPrefs.HasKey("PlayerPos")) {
            this.PlayerPos = JsonUtility.FromJson<Serialization<SceneType, Vector3>>(PlayerPrefs.GetString("PlayerPos"))
                .ToDictionary();
        } else {
            this.PlayerPos = new Dictionary<SceneType, Vector3>();
        }

        if (SceneChangeManager.Instance.IsNewDungeon) {
            this.RemovePlayerPos(SceneChangeManager.Instance.DungeonScene);
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
        this.OnLoadData?.Invoke();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneChangeManager.Instance.CurrentScene == SceneType.BigMap) {
            this.PlayerInBigMap = FindAnyObjectByType<Player>();
            
            SceneType currentDungeon = SceneChangeManager.Instance.DungeonScene;
            if (this.PlayerPos.ContainsKey(currentDungeon)) {
                this.PlayerInBigMap.transform.position = this.PlayerPos[currentDungeon];
            }else if (GameManager.Instance.IsBattleEnd) {
                this.PlayerInBigMap.transform.position = this.TempPlayerPos;
            }
        }
    }
    
    private void RemovePlayerPos(SceneType dungeon) {
        if (this.PlayerPos.ContainsKey(dungeon)) {
            this.PlayerPos.Remove(dungeon);
        }
    }

    public void ClearDungeonData() {
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
}


