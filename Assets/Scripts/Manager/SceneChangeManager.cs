
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour{

    [SerializeField] private AudioClip MainMenuBGM;
    [SerializeField] private AudioClip BigMapBGM;
    [SerializeField] private AudioClip AboutUsBGM;
    
    public static SceneChangeManager Instance;
    public SceneType CurrentScene{ get; private set; }

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
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
    }

    public void GoToScene(SceneType type){
        this.CurrentScene = type;
        SceneManager.LoadScene((int)type);
    }
}

