
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class PlayerSaveData {
    public float PlayerMoney = -1.0f;
    public SceneType CurrentDungeon = SceneType.None;
    public SerializableDictionary<SceneType, Vector3> PlayerPos = new();
    [SerializeReference] public List<string> InteractionObjectsEnds = new();
    [SerializeReference] public List<string> InteractionObjectsAvailable = new();
    public SerializableDictionary<string, float> DungeonHeroHealth = new();
    public SerializableDictionary<string, TaskCurrentData> CurrentTaskDataMap = new();
    public SerializableDictionary<string, int> OwnedPassiveEntries = new();
    [SerializeReference] public List<string> OwnedHeroes = new();
    [SerializeReference] public List<float> HeroEntanglementValues = new();
}


public class SaveDataManager : MonoBehaviour {

    [SerializeField] private int MaxSaveDataCount = 3;
    
    public static SaveDataManager Instance;
    
    public Player PlayerInBigMap { get; private set; }

    public PlayerSaveData PlayerData { get; private set; }

    public Action OnLoadData;

    private Vector3 TempPlayerPos = Vector3.zero;

    private SerializableDictionary<string, float> DupDungeonHealth = new();
    
    private int CurrentAutoSaveDataSlot = 0;
    public List<string> MutualSaveDataPaths { get; private set; } = new();

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        
        // TODO: TEMP -> CONVENIENT BATTLE TEST
        this.PlayerData = new PlayerSaveData();

        SceneManager.sceneLoaded += OnSceneLoaded;
        
        Debug.Log($"Save Data Path : {Application.persistentDataPath}");
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] autoSaveDatas = di.GetFiles("AutoSave_*.save");
        this.CurrentAutoSaveDataSlot = autoSaveDatas.Length % this.MaxSaveDataCount;
        
        FileInfo[] mutualSaveDatas = di.GetFiles("Save_*.save");
        foreach (FileInfo info in mutualSaveDatas) {
            this.MutualSaveDataPaths.Add(info.Name);
        }
    }

    private void Start() {
        SceneChangeManager.Instance.OnSceneChange += OnSceneChange;
    }

    private void OnSceneChange(SceneType oldScene, SceneType newScene) {
        if (oldScene == SceneType.BigMap && SceneTools.IsBattleScene(newScene)) {
            if(this.PlayerInBigMap) this.TempPlayerPos = this.PlayerInBigMap.transform.position;
        }
    }

    private void DungeonHealthDup() {
        this.DupDungeonHealth.Clear();
        foreach (KeyValuePair<string, float> pair in this.PlayerData.DungeonHeroHealth) {
            this.DupDungeonHealth.Add(pair.Key, pair.Value);
        }
    }

    private void SaveData(string savePath) {
        if (this.PlayerInBigMap) {
            SceneType currentDungeon = SceneChangeManager.Instance.DungeonScene;
            this.PlayerData.PlayerPos[currentDungeon] = this.PlayerInBigMap.transform.position;
        }
        this.PlayerData.PlayerMoney = GameManager.Instance.Money;

        string dataJson = JsonUtility.ToJson(this.PlayerData);
        File.WriteAllText(savePath, dataJson);
    }

    public void AutoSaveData() {
        string path = Path.Combine(Application.persistentDataPath, $"AutoSave_{this.CurrentAutoSaveDataSlot}.save");
        if (File.Exists(path)) {
            File.Delete(path);
        }

        this.CurrentAutoSaveDataSlot = (this.CurrentAutoSaveDataSlot + 1) % this.MaxSaveDataCount;
        this.SaveData(path);
    }

    public void MutualSaveData(int slot) {
        string fileName = $"Save_{slot}.save";
        if(!this.MutualSaveDataPaths.Contains(fileName)) this.MutualSaveDataPaths.Add(fileName);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        this.SaveData(path);
    }

    private void LoadData(string loadPath) {
        if (!File.Exists(loadPath)) {
            this.PlayerData = new PlayerSaveData();
        } else {
            string json = File.ReadAllText(loadPath);
            this.PlayerData = JsonUtility.FromJson<PlayerSaveData>(json);
        }
        
        if (SceneChangeManager.Instance.IsNewDungeon) {
            this.RemovePlayerPos(SceneChangeManager.Instance.DungeonScene);
        }
        this.OnLoadData?.Invoke();
    }

    public void LoadAutoSaveData() {
        int loadSlot = this.CurrentAutoSaveDataSlot == 0 ? this.MaxSaveDataCount - 1 : this.CurrentAutoSaveDataSlot - 1;
        string loadPath = Path.Combine(Application.persistentDataPath, $"AutoSave_{loadSlot}.save");
        this.LoadData(loadPath);
    }

    public void LoadMutualSaveData(int slot) {
        // TODO: Load Mutual Save UI
        string loadPath = Path.Combine(Application.persistentDataPath, $"Save_{slot}.save");
        this.LoadData(loadPath);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneChangeManager.Instance.CurrentScene == SceneType.BigMap) {
            this.PlayerInBigMap = FindAnyObjectByType<Player>();
            
            SceneType currentDungeon = SceneChangeManager.Instance.DungeonScene;
            if (GameManager.Instance.IsBattleEnd) {
                this.PlayerInBigMap.transform.position = this.TempPlayerPos;
            } else if (this.PlayerData.PlayerPos.ContainsKey(currentDungeon)) {
                this.PlayerInBigMap.transform.position = this.PlayerData.PlayerPos[currentDungeon];
            }
        }

        if (SceneTools.IsBattleScene(SceneChangeManager.Instance.CurrentScene)) {
            this.DungeonHealthDup();
            BattleManager.Instance.OnRewindBattle += () => {
                this.PlayerData.DungeonHeroHealth = this.DupDungeonHealth;
            };
        }
    }
    
    private void RemovePlayerPos(SceneType dungeon) {
        if (this.PlayerData.PlayerPos.ContainsKey(dungeon)) {
            this.PlayerData.PlayerPos.Remove(dungeon);
        }
    }

    public void ClearDungeonData() {
        this.PlayerData.DungeonHeroHealth.Clear();
    }

    public void ClearCurrentDungeon() {
        this.PlayerData.CurrentDungeon = SceneType.None;
    }

    public void SetInteractionObjectEnd(string objName) {
        if (!this.PlayerData.InteractionObjectsEnds.Contains(objName)) {
            this.PlayerData.InteractionObjectsEnds.Add(objName);
        }
    }

    public bool LoadInteractionObjectEnd(string objName) {
        return this.PlayerData.InteractionObjectsEnds.Contains(objName);
    }

    public bool LoadInteractionObjectAvailable(string dialogueName) {
        return this.PlayerData.InteractionObjectsAvailable.Contains(dialogueName);
    }

    public void SetInteractionObjectAvailable(string dialogueName) {
        if (!this.PlayerData.InteractionObjectsAvailable.Contains(dialogueName)) {
            this.PlayerData.InteractionObjectsAvailable.Add(dialogueName);
        }
    }

    public float GetHeroHealth(string heroName) {
        if (this.PlayerData.DungeonHeroHealth.ContainsKey(heroName)) {
            return this.PlayerData.DungeonHeroHealth[heroName];
        }
        return -1.0f;
    }

    public void SetHeroHealth(string heroName, float health) {

        if (this.PlayerData.DungeonHeroHealth.ContainsKey(heroName)) {
            this.PlayerData.DungeonHeroHealth[heroName] = health;
        } else {
            this.PlayerData.DungeonHeroHealth.Add(heroName, health);
        }
    }

    public void RecoverAllHeroHealth() {
        this.PlayerData.DungeonHeroHealth.Clear();
    }
}


