
using System;
using UnityEngine;

public class PassiveEntrySynthParentPanel : MonoBehaviour {

    [SerializeField] private PassiveEntryListPanel PassiveEntryListPanelUI;
    [SerializeField] private PassiveEntrySynthPanel PassiveEntrySynthPanelUI;

    private CanvasGroup PanelCanvasGroup;
    
    private void Awake() {
        this.PanelCanvasGroup = this.GetComponent<CanvasGroup>();
        
        this.PassiveEntryListPanelUI.OnPassiveEntryClicked += OnPassiveEntryClicked;
        this.PassiveEntrySynthPanelUI.OnReturnPassiveEntry += OnReturnPassiveEntry;
    }

    private void OnReturnPassiveEntry(string pName, int pCount) {
        this.PassiveEntryListPanelUI.ReturnPassiveEntry(pName, pCount);
    }

    private bool OnPassiveEntryClicked(string pName, int pCount) {
        return this.PassiveEntrySynthPanelUI.ChoosePassiveEntry(pName, pCount);
    }

    public void TransitionShow(bool show) {
        if (show && this.PanelCanvasGroup.alpha >= 0.9f) return;
        this.PanelCanvasGroup.alpha = show ? 1.0f : 0.0f;
        this.PanelCanvasGroup.interactable = show;
        this.PanelCanvasGroup.blocksRaycasts = show;

        if (show) {
            this.PassiveEntryListPanelUI.Show();
        } else {
            this.PassiveEntrySynthPanelUI.GoBackToNormal();
        }
    }
    
    
}


