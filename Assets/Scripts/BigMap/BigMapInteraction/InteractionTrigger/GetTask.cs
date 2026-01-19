
using System;
using UnityEngine;

public class GetTask : InteractionTrigger {

    [ScriptableObjectNameProp(typeof(TaskData), "TaskName")]
    [SerializeField] private string TaskName;
    [SerializeField] private Transform TaskPosition;

    protected override void TriggerAction() {
        TaskManager.Instance.AddTask(this.TaskName, this.TaskPosition ? this.TaskPosition.position : Vector3.zero);
    }
}

