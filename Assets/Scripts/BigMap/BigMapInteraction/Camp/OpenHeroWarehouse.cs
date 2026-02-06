

public class OpenHeroWarehouse : InteractionObject {
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActiveWhenAwake = true;
        base.Awake();
    }
    
    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.角色仓库;
    }

    protected override void Interaction() {
        HeroWarehouseManager.Instance.TransitionHeroWarehouseCanvas(true);
    }
}



