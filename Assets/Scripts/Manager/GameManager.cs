
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
        SaveMapManager.Instance.OnLoadData += () => {
            if (PlayerPrefs.HasKey("PlayerMoney")) {
                SetMoney(PlayerPrefs.GetFloat("PlayerMoney"));
            } else {
                SetMoney(this.InitialMoney);
            }
        };

        SaveMapManager.Instance.OnSaveData += () => {
            PlayerPrefs.SetFloat("PlayerMoney", this.Money);
        };
    }

    public void SetMoney(float money) {
        this.Money = money;
        if (BigMapUIManager.Instance) {
            BigMapUIManager.Instance.SetMoneyText(this.Money);
        }
    }

    private void Update(){
        Cursor.SetCursor(this.MouseCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (SceneTools.IsBattleScene(SceneChangeManager.Instance.CurrentScene)){
            BattleManager.Instance.SetBattleData(this.NextBattleData);
        }

        if (SceneChangeManager.Instance.CurrentScene != SceneType.BigMap){
            ResetBattleFlag();
        }
    }

    public void DungeonFail() {
        ResetBattleFlag();
        SceneChangeManager.Instance.GoToDungeon(SceneChangeManager.Instance.DungeonScene, true);
    }

    public void GoBackToCamp(bool isSaveRoom) {
        if (!isSaveRoom) {
            PlayerPrefs.DeleteKey("CurrentDungeon");
        }
        this.GoToScene(SceneType.Camp);
    }

    public void GoToBattle(BattleData battleData, bool showStartUI = true){
        this.NextBattleData = battleData;
        if (showStartUI) {
            if(GoToBattleSfx)
                AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.GoToBattleSfx);
            BigMapUIManager.Instance.ShowBattleStartUI(battleData.BattleScene, battleData.BattleBannarBackground, battleData.BattleImage, battleData.BattleText);
        } else {
            SceneChangeManager.Instance.GoToScene(battleData.BattleScene);   
        }
    }

    public void StartGame(){
        this.ResetBattleFlag();

        if (!PlayerPrefs.HasKey("CurrentDungeon")) {
            SaveMapManager.Instance.ClearDungeonData();
        }
        SaveMapManager.Instance.LoadData();

        if (PlayerPrefs.HasKey("CurrentDungeon")
            && Enum.TryParse(PlayerPrefs.GetString("CurrentDungeon"), out SceneType dungeon)) {
            SceneChangeManager.Instance.GoToDungeon(dungeon);
        } else {
            // TODO: If Not Play Newbie Dungeon -> Go to Newbie Dungeon
            // TODO: Else GO TO CAMP   
            SceneChangeManager.Instance.GoToScene(SceneType.Camp);
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
