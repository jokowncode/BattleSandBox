
using UnityEngine;

public class GoBackToCamp : InteractionObject {

    [SerializeField] private bool IsSaveRoom = true;
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActiveWhenAwake = true;
        base.Awake();
    }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.营地;
    }

    protected override void Interaction() {
        SaveDataManager.Instance.AutoSaveData();
        GameManager.Instance.GoBackToCamp(this.IsSaveRoom);
    }
}

