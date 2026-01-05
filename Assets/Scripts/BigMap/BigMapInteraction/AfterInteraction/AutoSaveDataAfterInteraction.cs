
using System;
using UnityEngine;

public class AutoSaveDataAfterInteraction : MonoBehaviour {
    private void Awake() {
        if (this.TryGetComponent(out InteractionObject io)) {
            io.OnInteractionEnded += () => {
                // TODO: Multi Save Data -> Auto Save
                SaveMapManager.Instance.SaveData();
            };
        }
    }
}


