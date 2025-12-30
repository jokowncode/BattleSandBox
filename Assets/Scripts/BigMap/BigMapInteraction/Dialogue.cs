
using System;
using DialogueEditor;
using UnityEngine;

public class Dialogue : InteractionObject {

    [SerializeField] private DialogGraph Dialogs;
    [SerializeField] private bool CanRepeat = true;
    [SerializeField] private bool IsForce = false;
    [SerializeField] private bool IsFullScreen = true;
    [SerializeField] private bool NotReset = false;
    
    private bool IsCurrentConversation;

    protected override string GetName() {
        Vector3 pos = this.transform.position;
        string dungeonName = SceneChangeManager.Instance.CurrentDungeonName;
        return $"{dungeonName}_Dialogue_{pos.x}_{pos.y}_{pos.z}";
    }

    protected override void LoadBigMapData() {
        if (NotReset && GameManager.Instance.HasDungeonComplete(SceneChangeManager.Instance.DungeonScene)) {
            this.EndInteraction();
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

