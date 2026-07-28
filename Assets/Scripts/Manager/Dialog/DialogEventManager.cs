
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogEventManager : MonoBehaviour {

    public static DialogEventManager Instance;

    private Dictionary<string, Action> DialogEvents = new Dictionary<string, Action>();
    private Animator DialogAnimator;
    
    private void Awake() {
        if (Instance != null) {
            return;
        }
        Instance = this;

        this.DialogAnimator = this.GetComponent<Animator>();
        this.AddEvent("ShakeCamera", () => {
            this.DialogAnimator.SetTrigger(AnimationParams.Shake);
        });
        this.AddEvent("TurnRed", () => {
            this.DialogAnimator.SetTrigger(AnimationParams.Red);
        });
        this.AddEvent("GameOver", () => {
            GameManager.Instance.DungeonFail();
        });
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


