
using System;
using UnityEngine;

public class CampTrainInstruction : MonoBehaviour {
    private void Awake() {
        this.gameObject.SetActive(false);
        if (!SaveDataManager.Instance.DungeonIsComplete(SceneType.Dungeons_Level1)) return;
        if (SaveDataManager.Instance.PlayerData.IsCampTrainInstruction) return;
        this.gameObject.SetActive(true);
        Invoke(nameof(Disappear), 1.5f);
        SaveDataManager.Instance.PlayerData.IsCampTrainInstruction = true;
    }

    private void Disappear() {
        this.gameObject.SetActive(false);
    }
}


