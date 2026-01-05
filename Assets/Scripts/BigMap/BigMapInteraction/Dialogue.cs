
using System;
using DialogueEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Dialogue : InteractionObject {

    [SerializeField] private DialogGraph Dialogs;
    [SerializeField] private bool CanRepeat = true;
    [SerializeField] private bool IsForce = false;
    [SerializeField] private bool IsFullScreen = true;
    
    private bool IsCurrentConversation;

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.对话;
    }

    protected override void LoadBigMapData() {
        if (this.IsEnd) {
            this.IsForce = false;
        }
    }

    protected override void PlayerEnter() {
        if (this.IsEnd && CanRepeat) {
            this.EnableInteraction(true);
        }

        if (this.IsForce){
            this.Interaction();
        }
    }

    private void OnDialogEnded(){
        DialogManager.Instance.OnDialogEnded -= OnDialogEnded;
        if (!IsCurrentConversation) return;
        if (CanRepeat){
            IsCurrentConversation = false;
            this.EnableInteraction(true);
        }

        // this.IsEnd = true;
        if (!this.IsEnd) {
            this.IsForce = false;
            this.EndInteraction();
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
        this.EnableInteraction(false);
    }
}

