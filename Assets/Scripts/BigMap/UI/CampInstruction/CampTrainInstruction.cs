
using System;
using UnityEngine;

public class CampTrainInstruction : CampInstruction {
    protected override bool ShowCondition() {
        if (!SaveDataManager.Instance.DungeonIsComplete(SceneType.Dungeons_Level1)) return false;
        if (SaveDataManager.Instance.PlayerData.IsCampTrainInstruction) return false;
        return true;
    }

    protected override void AfterShow() {
        SaveDataManager.Instance.PlayerData.IsCampTrainInstruction = true;
    }
}


