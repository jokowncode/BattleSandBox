
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour{

    [SerializeField] private AudioClip MainMenuBGM;
    [SerializeField] private AudioClip BigMapBGM;
    [SerializeField] private AudioClip AboutUsBGM;

    [Header("Black Screen")] 
    [SerializeField] private float Duration = 1.0f;
    
    public static SceneChangeManager Instance;
    public SceneType CurrentScene{ get; private set; }

    public Action<SceneType, SceneType> OnSceneChange;
    private CanvasGroup BlackCanvasGroup;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        this.BlackCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (this.CurrentScene is SceneType.Main or SceneType.Tutorial){
            AudioManager.Instance.SetMainMusic(this.MainMenuBGM);
        }

        if (this.CurrentScene == SceneType.BigMap){
            AudioManager.Instance.SetMainMusic(this.BigMapBGM);
        }

        if (this.CurrentScene == SceneType.AboutUs){
            AudioManager.Instance.SetMainMusic(this.AboutUsBGM);
        }
        AudioManager.Instance.StopFootstep();
        this.BlackCanvasGroup.alpha = 0.0f;
    }

    public void GoToScene(SceneType type, bool isBlackScreen = false) {
        this.OnSceneChange?.Invoke(this.CurrentScene, type);
        this.CurrentScene = type;
        CameraManager.Instance.MainCamera.cullingMask = 0;
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

