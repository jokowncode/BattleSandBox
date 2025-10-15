
using System;
using UnityEngine;

public class StoreInstructionDialogEvent : MonoBehaviour {

    private Store CurrentStore;

    private void Awake() {
        this.CurrentStore = this.GetComponent<Store>();
    }

    private void Start() {
        DialogEventManager.Instance.AddEvent("ShowStoreInstruction", () => {
            this.CurrentStore.ShowStore();
        });
        
        DialogEventManager.Instance.AddEvent("HideStoreInstruction", () => {
            BigMapUIManager.Instance.HideStore();
        });
    }
}

