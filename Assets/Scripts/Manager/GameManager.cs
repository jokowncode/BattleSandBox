
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
    
    [Header("Debug")] 
    [SerializeField] private SceneType TestDungeon = SceneType.Dungeons_Level1;
    
    public static GameManager Instance;

    public float Money { get; private set; } = 0.0f;
        
    private BattleData NextBattleData;
    
    public bool IsBattleEnd{ get; private set; }
    public bool IsBattleVictory{ get; private set; }

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
        if (PlayerPrefs.HasKey("PlayerMoney")) {
            SetMoney(PlayerPrefs.GetFloat("PlayerMoney"));
        } else {
            SetMoney(this.InitialMoney);
        }
    }

    public void SetMoney(float money) {
        this.Money = money;
        if (BigMapUIManager.Instance) {
            BigMapUIManager.Instance.SetMoneyText(this.Money);
        }
    }

    private void OnDestroy() {
        // TODO: TEMP -> For Debug
        // PlayerPrefs.SetFloat("PlayerMoney", this.Money);
    }

    private void Update(){
        Cursor.SetCursor(this.MouseCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (SceneChangeManager.Instance.CurrentScene == SceneType.Battle){
            BattleManager.Instance.SetBattleData(this.NextBattleData);
        }

        if (SceneChangeManager.Instance.CurrentScene != SceneType.BigMap){
            ResetBattleFlag();
        }
    }

    public void DungeonFail() {
        // TODO: Load Newest Save
    }

    public void GoToBattle(BattleData battleData){
        this.NextBattleData = battleData;
        if(GoToBattleSfx)
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.GoToBattleSfx);
        BigMapUIManager.Instance.ShowBattleStartUI(battleData.BattleBannarBackground, battleData.BattleImage, battleData.BattleText);
        // SceneChangeManager.Instance.GoToScene(SceneType.Battle);
    }

    public void StartGame(){
        this.ResetBattleFlag();

        if (PlayerPrefs.HasKey("CurrentDungeon")
            && Enum.TryParse(PlayerPrefs.GetString("CurrentDungeon"), out SceneType dungeon)) {
            SceneChangeManager.Instance.GoToDungeon(dungeon);
        } else {
            // TODO: If Not Play Newbie Dungeon -> Go to Newbie Dungeon
            // TODO: Else GO TO CAMP   
            SceneChangeManager.Instance.GoToDungeon(this.TestDungeon);
        }
    }

    public void GoToMap(bool isBattleEnd, bool isBattleVictory){
        this.IsBattleEnd = isBattleEnd;
        this.IsBattleVictory = isBattleVictory;
        SceneChangeManager.Instance.GoToScene(SceneType.BigMap, true);
    }

    public void GoToMainMenu(){
        SceneChangeManager.Instance.GoToScene(SceneType.Main);
    }

    public void GoToTutorial(){
        SceneChangeManager.Instance.GoToScene(SceneType.Tutorial);
    }

    public void GoToAboutUs(){
        SceneChangeManager.Instance.GoToScene(SceneType.AboutUs);
    }

    public void ResetBattleFlag(){
        this.IsBattleEnd = false;
        this.IsBattleVictory = false;
    }
}
