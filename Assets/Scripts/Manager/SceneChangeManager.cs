
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
    
    [Header("Black Screen")] 
    [SerializeField] private float Duration = 1.0f;
    
    public static SceneChangeManager Instance;
    public SceneType CurrentScene{ get; private set; }

    public Action<SceneType, SceneType> OnSceneChange;

    private SceneType DungeonScene;
    private bool IsLoadDungeon = false;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // TODO: TEMP -> FOR DEBUG
        PlayerPrefs.DeleteAll();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (this.CurrentScene is SceneType.Main or SceneType.Tutorial){
            AudioManager.Instance.SetMainMusic(this.MainMenuBGM);
        }

        if (this.CurrentScene == SceneType.BigMap && !this.IsLoadDungeon){
            this.IsLoadDungeon = true;
            AudioManager.Instance.SetMainMusic(this.BigMapBGM);
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
    
    private void ClearDungeonData() {
        PlayerPrefs.DeleteKey("PlayerBigMapData");
        PlayerPrefs.DeleteKey("InteractionObjectEnd");
        PlayerPrefs.DeleteKey("AvailableDialogues");
    }

    public void GoToDungeon(SceneType dungeonType) {
        this.DungeonScene = dungeonType;
        if (!PlayerPrefs.HasKey("CurrentDungeon") 
            || PlayerPrefs.GetString("CurrentDungeon") != this.DungeonScene.ToString()) {
            PlayerPrefs.SetString("CurrentDungeon", this.DungeonScene.ToString());
            this.ClearDungeonData();
        }
        SaveMapManager.Instance.LoadData();
        this.GoToScene(SceneType.BigMap, true);
    }

    public void GoToScene(SceneType type, bool isBlackScreen = false) {
        if (type == SceneType.BigMap) {
            this.IsLoadDungeon = false;
        }

        this.OnSceneChange?.Invoke(this.CurrentScene, type);
        this.CurrentScene = type;
        StartCoroutine(SceneChangeCoroutine(type, isBlackScreen));
    }

    private IEnumerator SceneChangeCoroutine(SceneType type, bool isBlackScreen = false) {
        if (isBlackScreen) {
            for (float t = 0.0f; t <= this.Duration; t += Time.deltaTime) {
                this.BlackCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, t / this.Duration);
                yield return null;
            }
            this.BlackCanvasGroup.alpha = 1.0f;
        }
        SceneManager.LoadScene((int)type);
    }
}

