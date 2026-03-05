
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    [SerializeField] private AudioClip GoToBattleSfx;
    [SerializeField] private Texture2D MouseCursor;

    [Header("Player Initial Data")] 
    [SerializeField] private float InitialMoney = 200.0f;
    
    public static GameManager Instance;

    public float Money { get; private set; } = 0.0f;
        
    private BattleData NextBattleData;
    public bool IsTrainBattle { get; private set; } = false;

    public bool IsBattleEnd{ get; private set; }
    public bool IsBattleVictory{ get; private set; }

    public bool IsBattleDefeat => IsBattleEnd && !IsBattleVictory;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start() {
        SaveDataManager.Instance.OnLoadData += () => {
            SetMoney(SaveDataManager.Instance.PlayerData.PlayerMoney);
        };
    }

    public void SetMoney(float money) {
        this.Money = money < 0.0f ? this.InitialMoney : money;
    }

    private void Update(){
        Cursor.SetCursor(this.MouseCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (SceneTools.IsBattleScene(SceneChangeManager.Instance.CurrentScene)){
            BattleManager.Instance.SetBattleData(this.NextBattleData);
        }
    }

    public void DungeonFail() {
        this.ResetBattleFlag();
        SceneChangeManager.Instance.GoToDungeon(SceneChangeManager.Instance.DungeonScene, true);
    }

    public void GoBackToCamp(bool isSaveRoom) {
        if (!isSaveRoom) {
            SaveDataManager.Instance.CurrentDungeonComplete();
            // TODO: DEMO END -> GET ALL PASSIVE ENTRY AND TACTIC
            if (SceneChangeManager.Instance.DungeonScene == SceneType.Dungeons_Level1) {
                PassiveEntryWarehouseManager.Instance.GetAllPassiveEntry(8);                
                GoodsWarehouseManager.Instance.GetAllTactic(50);
                EntanglementManager.Instance.AllHeroFullEntanglement();
            }
        }
        this.GoToScene(SceneType.Camp);
    }

    public void GoToBattle(BattleData battleData, bool showStartUI = true, bool isTrain = false){
        this.NextBattleData = battleData;
        this.IsTrainBattle = isTrain;
        ResetBattleFlag();
        if (showStartUI) {
            if(GoToBattleSfx)
                AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.GoToBattleSfx);
            BigMapUIManager.Instance.ShowBattleStartUI(battleData.BattleScene, battleData.BattleBannarBackground, battleData.BattleImage, battleData.BattleText);
        } else {
            SceneChangeManager.Instance.GoToScene(battleData.BattleScene);   
        }
    }

    public void BattleEndGoBack(bool victory) {
        if (this.IsTrainBattle) {
            this.ResetBattleFlag();
            this.GoToScene(SceneType.Camp);
        } else {
            GoToMap(true, victory);
        }
    }

    public void StartGame(){
        // SaveDataManager.Instance.LoadLastAutoSaveData();
        SaveDataManager.Instance.NewGame();
        this.EnterGame();
    }

    public void ContinueGame() {
        if (SaveDataManager.Instance.HasSaveData) {
            SaveDataManager.Instance.ShowSaveLoadDataUI(false);
        }
    }

    public void EnterGame() {
        this.ResetBattleFlag();
        if (SceneTools.IsDungeonScene(SaveDataManager.Instance.PlayerData.CurrentDungeon)) {
            SceneChangeManager.Instance.GoToDungeon(SaveDataManager.Instance.PlayerData.CurrentDungeon);
        } else {
            if (!SaveDataManager.Instance.DungeonIsComplete(SceneType.Dungeons_Newbie)) {
                PlayerPrefs.DeleteAll();
                SceneChangeManager.Instance.GoToDungeon(SceneType.Dungeons_Newbie);      
            } else {
                SceneChangeManager.Instance.GoToScene(SceneType.Camp);
            }
        }
    }

    public void GoToMap(bool isBattleEnd, bool isBattleVictory){
        this.IsBattleEnd = isBattleEnd;
        this.IsBattleVictory = isBattleVictory;
        SceneChangeManager.Instance.GoToScene(SceneType.BigMap, true);
    }

    public void GoToScene(SceneType scene) {
        SceneChangeManager.Instance.GoToScene(scene);
    }

    public void ResetBattleFlag(){
        this.IsBattleEnd = false;
        this.IsBattleVictory = false;
    }
}
