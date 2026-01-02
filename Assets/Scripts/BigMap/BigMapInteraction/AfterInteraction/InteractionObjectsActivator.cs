
using System;
using UnityEngine;

public class InteractionObjectsActivator : MonoBehaviour {
    
    [SerializeField] private InteractionObject[] ActivateInteractionObjects;
    
    private void Awake() {
        if (this.TryGetComponent(out InteractionObject io)) {
            io.OnInteractionEnded += () => {
                if (ActivateInteractionObjects != null && ActivateInteractionObjects.Length != 0) {
                    foreach (InteractionObject obj in ActivateInteractionObjects) {
                        obj.Activate();
                    }
                }
            };
        }
    }
}


