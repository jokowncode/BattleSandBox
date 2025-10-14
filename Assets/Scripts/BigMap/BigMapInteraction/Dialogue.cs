
using System;
using DialogueEditor;
using UnityEngine;

public class Dialogue : InteractionObject{

    [SerializeField] private DialogGraph Dialogs;
    [SerializeField] private bool CanRepeat = true;
    [SerializeField] private bool IsForce = false;
    [SerializeField] private bool IsFullScreen = true;
    
    private bool IsCurrentConversation;
    private bool IsDialogue;

    protected override void OnTriggerEnter(Collider other){
        if (this.IsDialogue && !CanRepeat) return;
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

        this.IsDialogue = true;
    }

    protected override void Interaction(){
        if (DialogManager.Instance.IsInDialog) return;
        if (this.IsDialogue && !CanRepeat) return;
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

