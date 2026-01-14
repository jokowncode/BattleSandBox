
public class SaveData : InteractionObject {
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActiveWhenAwake = true;
        base.Awake();
    }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.存档;
    }

    protected override void Interaction() {
        SaveDataManager.Instance.ShowSaveLoadDataUI(true);
    }
}


