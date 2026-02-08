
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenPassiveEntrySynth : InteractionObject {

    [SerializeField] private PassiveEntrySynthPanel PanelUI;
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActiveWhenAwake = true;
        base.Awake();
    }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.词条合成;
    }

    protected override void Interaction() {
        if (!this.PanelUI) return;
        this.PanelUI.TransitionShow(true);
    }
}


