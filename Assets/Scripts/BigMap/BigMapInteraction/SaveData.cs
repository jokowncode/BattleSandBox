
public class SaveData : InteractionObject {
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActiveWhenAwake = true;
        base.Awake();
    }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.SaveData;
    }

    protected override void Interaction() {
        SaveMapManager.Instance.SaveData();
    }
}


