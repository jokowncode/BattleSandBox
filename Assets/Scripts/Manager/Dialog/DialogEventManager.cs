
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogEventManager : MonoBehaviour {

    public static DialogEventManager Instance;

    private Dictionary<string, Action> DialogEvents = new Dictionary<string, Action>();
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void AddEvent(string eventName, Action callback) {
        if (!DialogEvents.TryAdd(eventName, callback)) {
            DialogEvents[eventName] += callback;
        }
    }

    public void RaiseEvent(string eventName) {
        if (DialogEvents.TryGetValue(eventName, out Action callback)) {
            callback?.Invoke();
        }
    }
}


