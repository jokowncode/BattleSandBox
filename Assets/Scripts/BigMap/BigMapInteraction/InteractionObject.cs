
using System;
using UnityEngine;

public abstract class InteractionObject : MonoBehaviour{

    protected Player InAreaPlayer;
    
    protected virtual void Awake(){
        this.enabled = false;
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


