
using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
