
using System;
using UnityEngine;

public abstract class InteractionObject : MonoBehaviour {
    
    [Header("Available")]
    [SerializeField] protected bool IsActiveWhenAwake = true;
    
    protected Player InAreaPlayer;
    protected bool IsEnd = false;
    protected bool IsActive = false;

    protected bool IsEndCanEnableInteraction = false;
    
    public Action OnInteractionEnded;
    
    protected abstract string GetName();

    protected virtual void Awake() {
        this.IsActive = this.IsActiveWhenAwake;
    }

    private void Start() {
        this.IsEnd = SaveMapManager.Instance.LoadInteractionObjectEnd(this.GetName());
        if (!this.IsActiveWhenAwake) {
            this.IsActive = SaveMapManager.Instance.LoadInteractionObjectAvailable(this.GetName());
        }
        
        this.LoadBigMapData();
        this.enabled = false;
    }

    public virtual void Activate() {
        this.IsActive = true;
        SaveMapManager.Instance.SetInteractionObjectAvailable(this.GetName());
    }

    private void OnLoadMap() {
        // SaveMapManager.Instance.OnLoadMap -= OnLoadMap;
    }

    protected virtual void LoadBigMapData() { }

    protected void EndInteraction() {
        this.IsEnd = true;
        SaveMapManager.Instance.SetInteractionObjectEnd(this.GetName());
        this.OnInteractionEnded?.Invoke();
    }

    protected void EnableInteraction(bool enable) {
        if (!this.InAreaPlayer) return; 
        this.InAreaPlayer.TransitionInteractionTip(enable);
        this.enabled = enable;
    }

    private void OnTriggerEnter(Collider other) {
        if (!this.IsActive) return;
        if (!other.TryGetComponent(out Player player)) return;
        this.InAreaPlayer = player;
        if (!this.IsEnd || this.IsEndCanEnableInteraction) {
            this.EnableInteraction(true);
        }
        this.PlayerEnter();
    }

    protected virtual void PlayerEnter() { }

    protected virtual void OnTriggerExit(Collider other){
        if (!other.TryGetComponent(out Player _)) return;
        this.EnableInteraction(false);
        this.InAreaPlayer = null;
    }

    protected abstract void Interaction();
    
    protected virtual void Update(){
        if (Input.GetKeyDown(KeyCode.E)){
            Interaction();
        }
    }
}


