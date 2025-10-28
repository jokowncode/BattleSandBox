
using System;
using UnityEngine;

public abstract class InteractionObject : MonoBehaviour{

    protected Player InAreaPlayer;
    protected bool IsEnd = false;

    public Action OnInteractionEnded;
    
    protected virtual void Awake(){
        this.enabled = false;
    }

    protected void EndInteraction() {
        this.IsEnd = true;
        this.OnInteractionEnded?.Invoke();
    }

    protected virtual void OnTriggerEnter(Collider other){
        if (!other.TryGetComponent(out Player player)) return;
        player.TransitionInteractionTip(true);
        this.InAreaPlayer = player;
        this.enabled = true;
    }

    protected virtual void OnTriggerExit(Collider other){
        if (!other.TryGetComponent(out Player player)) return;
        player.TransitionInteractionTip(false);
        this.InAreaPlayer = null;
        this.enabled = false;
    }

    protected abstract void Interaction();
    
    protected virtual void Update(){
        if (Input.GetKeyDown(KeyCode.E)){
            Interaction();
        }
    }
}


