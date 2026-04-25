
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
            StoreUI.Instance.StartInstructionMode();
        });
        
        DialogEventManager.Instance.AddEvent("ProceedStoreInstruction", () => {
            DialogManager.Instance.TransitionPause(true);
            DialogManager.Instance.TransitionClickArea(false);
            StoreUI.Instance.StoreInstructionMode(0, () => {
                DialogManager.Instance.TransitionPause(false);
                DialogManager.Instance.TransitionClickArea(true);
                DialogManager.Instance.Next();
            });
        });
        
        DialogEventManager.Instance.AddEvent("HideStoreInstruction", () => {
            StoreUI.Instance.HideStoreUI();
        });
    }
}

