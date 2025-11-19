
using System;
using DialogueEditor;
using UnityEngine;

public class Dialogue : InteractionObject {

    [SerializeField] private string DialogueName;
    [SerializeField] private DialogGraph Dialogs;
    [SerializeField] private bool CanRepeat = true;
    [SerializeField] private bool IsForce = false;
    [SerializeField] private bool IsFullScreen = true;
    
    [Header("Next Dialog")]
    [SerializeField] private bool IsActiveWhenAwake = true;
    [SerializeField] private Dialogue NextActiveDialogue;
    
    private bool IsCurrentConversation;
    private bool IsActive = false;

    protected override string GetName() {
        return this.DialogueName;
    }

    protected override void Awake() {
        base.Awake();
        this.IsActive = this.IsActiveWhenAwake;
        if (SaveMapManager.Instance.IsFirstLoad) {
            SaveMapManager.Instance.OnLoadMap += OnLoadMap;
        }
    }

    private void OnLoadMap() {
        if (!this.IsActiveWhenAwake) {
            this.IsActive = SaveMapManager.Instance.DialoguesAvailable(this.GetName());
        }
    }

    public void Activate() {
        this.IsActive = true;
        SaveMapManager.Instance.SaveAvailableDialogue(this.GetName());
    }

    protected override void OnTriggerEnter(Collider other) {
        if (!this.IsActive) return;
        if (this.IsEnd && !CanRepeat) return;
        if (!other.CompareTag("Player")) return;
        base.OnTriggerEnter(other);
        if (this.IsForce){
            TriggerDialogue();
        }
    }

    private void OnDialogEnded(){
        DialogManager.Instance.OnDialogEnded -= OnDialogEnded;
        if (!IsCurrentConversation) return;
        if (CanRepeat){
            IsCurrentConversation = false;
            this.InAreaPlayer.TransitionInteractionTip(true);
        }

        // this.IsEnd = true;
        if (!this.IsEnd) {
            this.EndInteraction();
            if (NextActiveDialogue) {
                NextActiveDialogue.Activate();
            }
        }
    }

    protected override void Interaction(){
        if (DialogManager.Instance.IsInDialog) return;
        if (this.IsEnd && !CanRepeat) return;
        if (IsCurrentConversation) return;
        TriggerDialogue();
    }

    private void TriggerDialogue(){
        IsCurrentConversation = true;
        // ConversationManager.Instance.StartConversation(this.Dialog);
        DialogManager.Instance.OnDialogEnded += OnDialogEnded;
        DialogManager.Instance.PlayNewDialog(this.Dialogs, this.IsFullScreen);
        this.InAreaPlayer.TransitionInteractionTip(false);
    }
}

