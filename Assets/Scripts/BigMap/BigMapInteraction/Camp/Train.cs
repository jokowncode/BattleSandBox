
using UnityEngine;

public class Train : InteractionObject {

    [SerializeField] private BattleData[] RandomBattleDatas;
    
    protected override void Awake() {
        this.IsBindTask = false;
        this.IsActiveWhenAwake = true;
        base.Awake();
    }
    
    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.训练室;
    }

    protected override void Interaction() {
        if (!SaveDataManager.Instance.DungeonIsComplete(SceneType.Dungeons_Level1)) {
            SceneChangeManager.Instance.AddGameTip("当前还未解锁！");
            return;
        }
        
        int index = Random.Range(0, RandomBattleDatas.Length);
        GameManager.Instance.GoToBattle(this.RandomBattleDatas[index], false, true);
    }
}



