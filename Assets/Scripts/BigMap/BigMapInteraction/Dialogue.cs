
using System;
using DialogueEditor;
using UnityEngine;

public class Dialogue : InteractionObject{

    [SerializeField] private NPCConversation Dialog;
    [SerializeField] private bool CanRepeat = true;
    [SerializeField] private bool IsForce = false;

    private bool IsCurrentConversation;
    private bool IsDialogue;
    
    private void Start(){
        ConversationManager.OnConversationEnded += OnConversationEnded;
    }

    protected override void OnTriggerEnter(Collider other){
        if (this.IsDialogue && !CanRepeat) return;
        if (!other.CompareTag("Player")) return;
        base.OnTriggerEnter(other);
        if (this.IsForce){
            TriggerDialogue();
        }
    }

    private void OnConversationEnded(){
        if (!IsCurrentConversation) return;
        if (CanRepeat){
            IsCurrentConversation = false;
            this.InAreaPlayer.TransitionInteractionTip(true);
        }

        this.IsDialogue = true;
        this.InAreaPlayer.TransMove(true);
    }

    protected override void Interaction(){
        if (this.IsDialogue && !CanRepeat) return;
        if (IsCurrentConversation) return;
        TriggerDialogue();
    }

    private void TriggerDialogue(){
        IsCurrentConversation = true;
        ConversationManager.Instance.StartConversation(this.Dialog);
        this.InAreaPlayer.TransitionInteractionTip(false);
        this.InAreaPlayer.TransMove(false);
    }
}

