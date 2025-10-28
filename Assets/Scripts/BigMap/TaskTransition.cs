
using System;
using UnityEngine;

public class TaskTransition : MonoBehaviour {

    [SerializeField] private TaskData NextTaskData;

    private InteractionObject Interaction;
    
    private void Awake() {
        this.Interaction = GetComponent<InteractionObject>();
        if (this.Interaction) {
            this.Interaction.OnInteractionEnded += OnInteractionEnded;
        }
    }

    private void OnInteractionEnded() {
        this.Interaction.OnInteractionEnded -= OnInteractionEnded;
        BigMapUIManager.Instance.ShowNewTask(this.NextTaskData);
    }
}

