
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogClick : MonoBehaviour, IPointerClickHandler {

    private DialogManager Manager;

    private void Awake() {
        this.Manager = this.GetComponentInParent<DialogManager>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (this.Manager.IsAutoPlay || this.Manager.IsExplore || this.Manager.IsVideo) return;
        this.Manager.Next();
    }
}

