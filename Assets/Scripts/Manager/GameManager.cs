
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    [SerializeField] private AudioClip GoToBattleSfx;
    [SerializeField] private Texture2D MouseCursor;

    // TODO: TEMP -> Link UI (Dungeon Choose)
    [SerializeField] private SceneType TestDungeon = SceneType.Dungeons_Level1;
    
    public static GameManager Instance;

    public float Money { get; private set; } = 0.0f;
        
    private BattleData NextBattleData;
    
    public bool IsBattleEnd{ get; private set; }
    public bool IsBattleVictory{ get; private set; }

    private SceneType GoToDungeon;

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
            SetMoney(0.0f);
        }
        
        // TODO: TEMP -> For Debug
        SetMoney(200.0f);
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


    public void LoadDungeonSubScene(Action<float> progressCallback = null) {
        StartCoroutine(LoadDungeonSubSceneCoroutine(progressCallback));
    }

    private IEnumerator LoadDungeonSubSceneCoroutine(Action<float> progressCallback = null) {
        AsyncOperation ao = SceneManager.LoadSceneAsync((int)this.GoToDungeon, LoadSceneMode.Additive);
        if (ao == null) yield break;
        if (progressCallback == null) yield break;

        while (ao.progress <= 1.0f) {
            progressCallback?.Invoke(ao.progress);
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (SceneChangeManager.Instance.CurrentScene == SceneType.Battle){
            BattleManager.Instance.SetBattleData(this.NextBattleData);
        }

        if (SceneChangeManager.Instance.CurrentScene != SceneType.BigMap){
            ResetBattleFlag();
        }
    }

    public void GoToBattle(BattleData battleData){
        this.NextBattleData = battleData;
        if(GoToBattleSfx)
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.GoToBattleSfx);
        BigMapUIManager.Instance.ShowBattleStartUI(battleData.BattleBannarBackground, battleData.BattleImage, battleData.BattleText);
        // SceneChangeManager.Instance.GoToScene(SceneType.Battle);
    }

    public void StartGame(){
        // TODO: TEMP -> Link Camp UI
        this.GoToDungeon = this.TestDungeon;
        if (!PlayerPrefs.HasKey("CurrentDungeon") 
            || PlayerPrefs.GetString("CurrentDungeon") != this.GoToDungeon.ToString()) {
            PlayerPrefs.SetString("CurrentDungeon", this.GoToDungeon.ToString());
            this.ClearDungeonData();
        }
        SaveMapManager.Instance.LoadData();
        this.GoToMap(false, false);    
    }

    private void ClearDungeonData() {
        PlayerPrefs.DeleteKey("PlayerBigMapData");
        PlayerPrefs.DeleteKey("InteractionObjectEnd");
        PlayerPrefs.DeleteKey("AvailableDialogues");
        
        // TODO: Clear Store Data Here?
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
