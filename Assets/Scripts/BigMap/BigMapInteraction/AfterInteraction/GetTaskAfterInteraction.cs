
using System;
using UnityEngine;

public class GetTaskAfterInteraction : MonoBehaviour {

    [SerializeField] private string TaskName;
    [SerializeField] private Transform TaskPosition;

    private void Awake() {
        if (this.TryGetComponent(out InteractionObject io)) {
            io.OnInteractionEnded += () => {
                TaskManager.Instance.AddTask(this.TaskName, this.TaskPosition ? this.TaskPosition.position : Vector3.zero);
            };
        }
    }
}

