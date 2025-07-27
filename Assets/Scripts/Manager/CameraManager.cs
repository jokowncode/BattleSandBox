
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour {

    public static CameraManager Instance;

    private Camera MainCamera;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        MainCamera = Camera.main;
    }

    public void FollowTarget() { }
    public void ShakeCamera(float duration, float magnitude) { }
}

