
using System;
using UnityEngine;

public abstract class InteractionObject : MonoBehaviour{

    protected Player InAreaPlayer;
    protected bool IsEnd = false;

    public Action OnInteractionEnded;

    protected abstract string GetName();
    
    protected virtual void Awake(){
        SaveMapManager.Instance.OnLoadMap += OnLoadMap;
        this.enabled = false;
    }

    private void OnLoadMap() {
        SaveMapManager.Instance.OnLoadMap -= OnLoadMap;
        this.IsEnd = SaveMapManager.Instance.LoadInteractionObject(this.GetName());
        this.LoadBigMapData();
    }

    protected virtual void LoadBigMapData() { }

    protected void EndInteraction() {
        this.IsEnd = true;
        SaveMapManager.Instance.SaveInteractionObject(this.GetName(), this.IsEnd);
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


