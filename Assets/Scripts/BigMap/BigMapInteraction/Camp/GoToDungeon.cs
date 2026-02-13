
using UnityEngine;

public class GoToDungeon : InteractionObject {

    [SerializeField] private SceneType Dungeon = SceneType.Dungeons_Level1;
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActiveWhenAwake = true;
        base.Awake();
    }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.出发;
    }

    protected override void Interaction() {
        SaveDataManager.Instance.AutoSaveData();
        SceneChangeManager.Instance.GoToDungeon(this.Dungeon);
    }
}


