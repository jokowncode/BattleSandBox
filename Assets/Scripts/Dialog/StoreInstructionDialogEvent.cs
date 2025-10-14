
using System;
using UnityEngine;

public class StoreInstructionDialogEvent : MonoBehaviour {
    private void Start() {
        DialogEventManager.Instance.AddEvent("ShowStore", () => {
            BigMapUIManager.Instance.TransitionStore(true);
        });
        
        DialogEventManager.Instance.AddEvent("HideStore", () => {
            BigMapUIManager.Instance.TransitionStore(false);
        });
    }
}

