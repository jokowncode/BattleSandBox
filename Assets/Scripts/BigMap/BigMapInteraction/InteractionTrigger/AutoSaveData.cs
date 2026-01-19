
using System;
using UnityEngine;

public class AutoSaveData : InteractionTrigger {
    protected override void TriggerAction() {
        SaveDataManager.Instance.AutoSaveData();
    }
}


