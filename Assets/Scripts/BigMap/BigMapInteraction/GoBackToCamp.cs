
using UnityEngine;

public class GoBackToCamp : InteractionObject {

    [SerializeField] private bool IsSaveRoom = true;
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActiveWhenAwake = true;
        base.Awake();
    }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.GoBackToCamp;
    }

    protected override void Interaction() {
        // TODO: Multi Save Data -> Auto Save
        SaveMapManager.Instance.SaveData();
        GameManager.Instance.GoBackToCamp(this.IsSaveRoom);
    }
}

