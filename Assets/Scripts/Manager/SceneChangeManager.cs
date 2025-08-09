
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour{

    public static SceneChangeManager Instance;
    public SceneType CurrentScene{ get; private set; }

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void GoToScene(SceneType type){
        this.CurrentScene = type;
        SceneManager.LoadScene((int)type);
    }
}

