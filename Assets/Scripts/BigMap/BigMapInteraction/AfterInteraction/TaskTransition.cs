
using System;
using UnityEngine;

public class TaskTransition : MonoBehaviour {

    [SerializeField] private Transform NextTaskLocation;
    
    private InteractionObject Interaction;
    
    private void Awake() {
        this.Interaction = GetComponent<InteractionObject>();
        if (this.Interaction && this.Interaction.IsBindTask) {
            this.Interaction.OnInteractionEnded += OnInteractionEnded;
        }
    }

    private void OnInteractionEnded() {
        this.Interaction.OnInteractionEnded -= OnInteractionEnded;
        TaskManager.Instance.NextTask(this.Interaction.OwnedTaskName, this.NextTaskLocation);
    }
}

