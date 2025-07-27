
using System;
using UnityEngine;

public class PassiveEntryWarehouseManager : MonoBehaviour {

    public static PassiveEntryWarehouseManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
