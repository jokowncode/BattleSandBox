
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour{

    [Header("Music")]
    [SerializeField] private AudioClip MainMenuBGM;
    [SerializeField] private AudioClip BigMapBGM;
    [SerializeField] private AudioClip AboutUsBGM;

    [Header("UI")] 
    [SerializeField] private CanvasGroup BlackCanvasGroup;
    [SerializeField] private LoadingUI Loading;
    [SerializeField] private GameTipContainer GameTip;
    
    [Header("Black Screen")] 
    [SerializeField] private float Duration = 1.0f;
    
    public static SceneChangeManager Instance;
    public SceneType CurrentScene{ get; private set; }

    public Action<SceneType, SceneType> OnSceneChange;

    public SceneType DungeonScene { get; private set; }
    private bool IsLoadDungeon = false;

    public string CurrentDungeonName => this.DungeonScene.ToString();

    public bool IsNewDungeon { get; private set; } = false;

    private bool IsLoadScene = false;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void AddGameTip(string tipText) {
        this.GameTip.AddTip(tipText);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        this.IsLoadScene = false;
        if (this.CurrentScene is SceneType.Main or SceneType.Tutorial){
            AudioManager.Instance.SetMainMusic(this.MainMenuBGM);
        }

        if (this.CurrentScene == SceneType.BigMap && !this.IsLoadDungeon){
            this.IsLoadDungeon = true;
            AudioManager.Instance.SetMainMusic(this.BigMapBGM);
            BigMapUIManager.Instance.TaskList.UpdateTaskUI();
            if (this.DungeonScene == SceneType.Dungeons_Newbie) {
                PlayerPrefs.DeleteAll();
            }
            StartCoroutine(AsyncLoadSceneCallback(this.DungeonScene));
        }

        if (this.CurrentScene == SceneType.AboutUs){
            AudioManager.Instance.SetMainMusic(this.AboutUsBGM);
        }
        AudioManager.Instance.StopFootstep();
        this.BlackCanvasGroup.alpha = 0.0f;
    }
    
    private IEnumerator AsyncLoadSceneCallback(SceneType scene, LoadSceneMode mode = LoadSceneMode.Additive) {
        this.Loading.Transition(true);
        this.Loading.UpdateLoadingProgress(0.0f);

        AsyncOperation ao = SceneManager.LoadSceneAsync((int)scene, mode);
        if (ao == null) yield break;

        while (!ao.isDone) {
            this.Loading.UpdateLoadingProgress(ao.progress);
            yield return null;
        }
        this.Loading.UpdateLoadingProgress(1.0f);
        this.Loading.Transition(false);
    }

    public void GoToDungeon(SceneType dungeonType, bool reloadData = false) {
        if (!SceneTools.IsDungeonScene(dungeonType)) {
            return;
        }
        this.DungeonScene = dungeonType;
        if (reloadData) SaveDataManager.Instance.LoadLastAutoSaveData();
        
        SceneType dungeon = SaveDataManager.Instance.PlayerData.CurrentDungeon;
        this.IsNewDungeon = dungeon == SceneType.None || dungeon != this.DungeonScene;
        if (IsNewDungeon) {
            SaveDataManager.Instance.PlayerData.CurrentDungeon = this.DungeonScene;
            if (!SaveDataManager.Instance.DungeonIsComplete(this.DungeonScene)) {
                TaskManager.Instance.AddDungeonBindTask(this.DungeonScene);
            }
        }
        this.GoToScene(SceneType.BigMap, true);
    }

    public void GoToScene(SceneType type, bool isBlackScreen = false) {
        if (this.IsLoadScene) return;
        this.IsLoadScene = true;
        
        if (type == SceneType.BigMap) {
            this.IsLoadDungeon = false;
        }

        if (SceneTools.IsBattleScene(this.CurrentScene) && type == SceneType.BigMap) {
            this.IsNewDungeon = false;
        }

        this.OnSceneChange?.Invoke(this.CurrentScene, type);
        this.CurrentScene = type;
        StartCoroutine(SceneChangeCoroutine(type, isBlackScreen));
    }

    public IEnumerator CompleteBlackScreenCoroutine(float start, float end, Action dosomething = null) {
        yield return BlackScreenCoroutine(start, end);
        dosomething?.Invoke();
        yield return BlackScreenCoroutine(end, start);
    }
    
    private IEnumerator BlackScreenCoroutine(float start, float end) {
        this.BlackCanvasGroup.alpha = start;
        for (float t = 0.0f; t <= this.Duration; t += Time.deltaTime) {
            this.BlackCanvasGroup.alpha = Mathf.Lerp(start, end, t / this.Duration);
            yield return null;
        }
        this.BlackCanvasGroup.alpha = end;
    }

    private IEnumerator SceneChangeCoroutine(SceneType type, bool isBlackScreen = false) {
        if (isBlackScreen) {
            yield return BlackScreenCoroutine(0.0f, 1.0f);
        }
        SceneManager.LoadScene((int)type);
    }
}

