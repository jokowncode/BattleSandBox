
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
        if (SaveDataManager.Instance.DungeonIsComplete(this.Dungeon)) {
            SceneChangeManager.Instance.AddGameTip("后面的内容暂未解锁！");
            return;
        }
        SaveDataManager.Instance.AutoSaveData();
        SceneChangeManager.Instance.GoToDungeon(this.Dungeon);
    }
}


