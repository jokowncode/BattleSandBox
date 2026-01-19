
using System;
using UnityEngine;

public enum InteractionTriggerType {
    Before,
    After
}

public abstract class InteractionTrigger : MonoBehaviour {
    
    [SerializeField] private InteractionTriggerType TriggerType = InteractionTriggerType.After;

    protected InteractionObject CurrentIO;
    
    private void Awake() {
        this.CurrentIO = this.GetComponent<InteractionObject>();
        if (this.CurrentIO) {
            switch (this.TriggerType) {
                case InteractionTriggerType.Before:
                    this.CurrentIO.OnInteractionPre += this.TriggerAction;
                    break;
                case InteractionTriggerType.After:
                    this.CurrentIO.OnInteractionEnded += this.TriggerAction;
                    break;
            }
        }
    }

    protected abstract void TriggerAction();
}


