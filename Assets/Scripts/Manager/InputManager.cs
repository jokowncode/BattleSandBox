
using System;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Update() {
        // TODO: INPUT SYSTEM ?
        /*if (Input.GetKeyDown(KeyCode.Escape)) {
            if (SceneChangeManager.Instance.CurrentScene != SceneType.Camp &&
                SceneChangeManager.Instance.CurrentScene != SceneType.BigMap) return;
            SaveDataManager.Instance.ShowSaveLoadDataUI(false);
        }*/
    }
}


