
using UnityEngine;

public class OpenGoodsWarehouse : InteractionObject {
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActiveWhenAwake = true;
        base.Awake();
    }
    
    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.道具仓库;
    }

    protected override void Interaction() {
        GoodsWarehouseManager.Instance.TransitionGoodsPanel(true);
    }
}


