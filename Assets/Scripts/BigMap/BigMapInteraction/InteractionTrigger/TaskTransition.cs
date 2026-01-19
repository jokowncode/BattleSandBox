
using System;
using UnityEngine;

public class TaskTransition : InteractionTrigger {

    [SerializeField] private Transform NextTaskLocation;

    protected override void TriggerAction() {
        if (!this.CurrentIO.IsBindTask) return;
        TaskManager.Instance.NextTask(this.CurrentIO.TaskName, this.NextTaskLocation);
    }
}

