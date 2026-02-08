
using System;
using UnityEngine;

public class PassiveEntrySynthPanel : MonoBehaviour {

    [SerializeField] private PassiveEntryListPanel PassiveEntryListPanelUI;

    private CanvasGroup PanelCanvasGroup;
    
    private void Awake() {
        this.PanelCanvasGroup = this.GetComponent<CanvasGroup>();
        TransitionShow(false);
    }

    public void TransitionShow(bool show) {
        if (show && this.PanelCanvasGroup.alpha >= 0.9f) return;
        this.PanelCanvasGroup.alpha = show ? 1.0f : 0.0f;
        this.PanelCanvasGroup.interactable = show;
        this.PanelCanvasGroup.blocksRaycasts = show;

        if (show) {
            this.PassiveEntryListPanelUI.Show();
        }
    }
    
    
}


