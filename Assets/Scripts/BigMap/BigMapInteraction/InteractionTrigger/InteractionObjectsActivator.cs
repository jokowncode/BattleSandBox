
using System;
using UnityEngine;

public class InteractionObjectsActivator : InteractionTrigger {
    
    [SerializeField] private InteractionObject[] ActivateInteractionObjects;

    protected override void TriggerAction() {
        if (ActivateInteractionObjects != null && ActivateInteractionObjects.Length != 0) {
            foreach (InteractionObject obj in ActivateInteractionObjects) {
                obj.Activate();
            }
        }
    }
}


