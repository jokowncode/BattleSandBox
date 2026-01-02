
using UnityEngine;

public class GoBackToCamp : InteractionObject {

    [SerializeField] private bool IsSaveRoom = true;
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActive = true;
        base.Awake();
    }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.GoBackToCamp;
    }

    protected override void Interaction() {
        GameManager.Instance.GoBackToCamp(this.IsSaveRoom);
    }
}

