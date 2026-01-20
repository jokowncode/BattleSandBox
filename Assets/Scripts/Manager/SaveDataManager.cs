
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
    [SerializeReference] public List<SceneType> CompleteDungeons = new();
    public SerializableDictionary<string, int> OwnedConsumedGoods = new();
}


public class SaveDataManager : MonoBehaviour {

    [SerializeField] private int MaxSaveDataCount = 3;
    [SerializeField] private SaveLoadDataUI SaveLoadDataUI;
    
    public static SaveDataManager Instance;
    
    public Player PlayerInBigMap { get; private set; }

    public PlayerSaveData PlayerData { get; private set; }

    public Action OnLoadData;

    private Vector3 TempPlayerPos = Vector3.zero;

    private SerializableDictionary<string, float> DupDungeonHealth = new();

    public Queue<string> AutoSaveDataPaths { get; private set; } = new();

    public Dictionary<int, string> MutualSaveDataPathMap { get; private set; } = new();

    private long AlreadyPlayTime = 0;
    private long LoadTimeStamp = 0;
    
    public bool HasSaveData => AutoSaveDataPaths.Count != 0 || MutualSaveDataPathMap.Count != 0;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
#if TEST_BATTLE
        this.PlayerData = new PlayerSaveData();
#endif
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        Debug.Log($"Save Data Path : {Application.persistentDataPath}");
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] autoSaveDatas = di.GetFiles("AutoSave_*.save");
        foreach (FileInfo info in autoSaveDatas) {
            this.AutoSaveDataPaths.Enqueue(info.Name);
        }
        
        FileInfo[] mutualSaveDatas = di.GetFiles("Save_*.save");
        foreach (FileInfo info in mutualSaveDatas) {
            string fileName = info.Name;
            int start = fileName.LastIndexOf("_") + 1;
            int end = fileName.LastIndexOf(".");
            int length = end - start;
            int slot = int.Parse(fileName.Substring(start, length));
            this.MutualSaveDataPathMap.Add(slot, info.Name);
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

    private string GetSaveDataFileName(bool isAutoSave, int slot = 0) {
        string result = isAutoSave ? "AutoSave" : "Save";
        long currentTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        result += "_" + currentTimeStamp;
        long diff = (currentTimeStamp - this.LoadTimeStamp) / 1000 + this.AlreadyPlayTime;
        result += "_" + diff;
        
        string dungeonName = PlayerData.CurrentDungeon.ToString();
        if (SceneTools.IsDungeonScene(PlayerData.CurrentDungeon)) {
            dungeonName = dungeonName.Split("_")[1];
        } else {
            dungeonName = "Camp";
        }
        result += "_" + dungeonName;
        result += $"_{slot}";
        return result + ".save";
    }

    public void AutoSaveData() {
        if (this.AutoSaveDataPaths.Count >= this.MaxSaveDataCount) {
            string deleteFileName = this.AutoSaveDataPaths.Dequeue();
            string deletePath = Path.Combine(Application.persistentDataPath, deleteFileName);
            if (File.Exists(deletePath)) {
                File.Delete(deletePath);
            }
        }
        
        string fileName = GetSaveDataFileName(true);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        this.AutoSaveDataPaths.Enqueue(fileName);
        this.SaveData(path);
    }

    public string MutualSaveData(int slot) {
        string fileName = GetSaveDataFileName(false, slot);
        if (this.MutualSaveDataPathMap.ContainsKey(slot)) {
            string deletePath = Path.Combine(Application.persistentDataPath, this.MutualSaveDataPathMap[slot]);
            if (File.Exists(deletePath)) {
                File.Delete(deletePath);
            }
        }
        this.MutualSaveDataPathMap[slot] = fileName;
        string path = Path.Combine(Application.persistentDataPath, fileName);
        this.SaveData(path);
        return fileName;
    }

    public void LoadData(string loadPath) {
        this.LoadTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        if (!File.Exists(loadPath)) {
            this.AlreadyPlayTime = 0;
            this.PlayerData = new PlayerSaveData();
        } else {
            this.AlreadyPlayTime = long.Parse(loadPath.Split("_")[2]);
            string json = File.ReadAllText(loadPath);
            this.PlayerData = JsonUtility.FromJson<PlayerSaveData>(json);
        }
        
        if (SceneChangeManager.Instance.IsNewDungeon) {
            this.RemovePlayerPos(SceneChangeManager.Instance.DungeonScene);
        }
        this.OnLoadData?.Invoke();
    }

    public void LoadLastAutoSaveData() {
        string fileName = this.AutoSaveDataPaths.LastOrDefault();
        fileName ??= "";
        string loadPath = Path.Combine(Application.persistentDataPath, fileName);
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
    
    public void ShowSaveLoadDataUI(bool isSaveData) {
        this.SaveLoadDataUI.TransitionShow(true, isSaveData);
    }
    
    private void RemovePlayerPos(SceneType dungeon) {
        if (this.PlayerData.PlayerPos.ContainsKey(dungeon)) {
            this.PlayerData.PlayerPos.Remove(dungeon);
        }
    }

    public void ClearDungeonData() {
        this.PlayerData.DungeonHeroHealth.Clear();
    }

    public void CurrentDungeonComplete() {
        if (!this.PlayerData.CompleteDungeons.Contains(this.PlayerData.CurrentDungeon)) {
            this.PlayerData.CompleteDungeons.Add(this.PlayerData.CurrentDungeon);
            TaskManager.Instance.RemoveDungeonBindTask(this.PlayerData.CurrentDungeon);
        }
        this.PlayerData.CurrentDungeon = SceneType.None;
    }

    public bool DungeonIsComplete(SceneType dungeon) {
        return this.PlayerData.CompleteDungeons.Contains(dungeon);
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

    public void RecoverHeroHealth(string heroName, float value, bool revive = false) {
        if (!this.PlayerData.DungeonHeroHealth.ContainsKey(heroName)) return;
        if (this.PlayerData.DungeonHeroHealth[heroName] == 0.0f && !revive) return;
        this.PlayerData.DungeonHeroHealth[heroName] += value;
    }

    public void RecoverAllHeroHealth() {
        this.PlayerData.DungeonHeroHealth.Clear();
    }
}


