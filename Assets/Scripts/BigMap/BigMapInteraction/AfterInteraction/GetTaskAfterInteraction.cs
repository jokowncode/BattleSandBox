
using System;
using UnityEngine;

public class GetTaskAfterInteraction : MonoBehaviour {

    [SerializeField] private string TaskName;

    private void Awake() {
        if (this.TryGetComponent(out InteractionObject io)) {
            io.OnInteractionEnded += () => {
                TaskManager.Instance.AddTask(this.TaskName);
            };
        }
    }
}

