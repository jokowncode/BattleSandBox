
using System;
using UnityEngine;

public abstract class InteractionObject : MonoBehaviour {

    protected enum InteractionObjType {
        商店,
        战斗,
        对话,
        障碍,
        存档,
        营地,
        回血,
        副本
    }
    
    [field: SerializeField] public string OwnedTaskName { get; protected set; } = "None";
    [field: SerializeField] public bool IsBindTask { get; protected set; } = true;

    [Header("Available")]
    [SerializeField] protected bool IsActiveWhenAwake = true;
    
    protected Player InAreaPlayer;
    protected bool IsEnd = false;
    protected bool IsActive = false;
    
    protected bool IsEndCanInteract = false;
    
    public Action OnInteractionEnded;
    public Action OnInteractionPre;

    protected string GetName() {
        Vector3 pos = this.transform.position;
        string dungeonName = SceneChangeManager.Instance.CurrentDungeonName;
        return $"{dungeonName}_{OwnedTaskName}_{GetInteractionObjType().ToString()}_{pos.x}_{pos.y}_{pos.z}";
    }

    protected abstract InteractionObjType GetInteractionObjType();

    protected virtual void Awake() {
        if (!this.IsBindTask) {
            this.IsActive = this.IsActiveWhenAwake;
        } else {
            this.IsActive = TaskManager.Instance.HasTask(this.OwnedTaskName) && this.IsActiveWhenAwake;
        }
    }

    protected virtual void Start() {
        this.IsEnd = SaveDataManager.Instance.LoadInteractionObjectEnd(this.GetName());
        if (!this.IsActive) {
            this.IsActive = SaveDataManager.Instance.LoadInteractionObjectAvailable(this.GetName());
        }
        
        this.LoadBigMapData();
        this.enabled = false;
    }

    public virtual void Activate() {
        this.IsActive = true;
        SaveDataManager.Instance.SetInteractionObjectAvailable(this.GetName());
    }

    protected virtual void LoadBigMapData() { }

    protected void EndInteraction() {
        if (this.IsEnd) return;
        this.IsEnd = true;
        SaveDataManager.Instance.SetInteractionObjectEnd(this.GetName());
        this.OnInteractionEnded?.Invoke();
    }

    protected void EnableInteraction(bool enable) {
        if (!this.InAreaPlayer) return; 
        this.InAreaPlayer.TransitionInteractionTip(enable, this.GetInteractionObjType().ToString());
        this.enabled = enable;
    }

    private void OnTriggerEnter(Collider other) {
        if (!this.IsActive && this.IsBindTask) {
            this.IsActive = TaskManager.Instance.HasTask(this.OwnedTaskName) && this.IsActiveWhenAwake;
        }
        
        if (!this.IsActive) return;
        if (!other.TryGetComponent(out Player player)) return;
        this.InAreaPlayer = player;
        if (!this.IsEnd || this.IsEndCanInteract) {
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
        if (Input.GetKeyDown(KeyCode.E)) {
            OnInteractionPre?.Invoke();
            Interaction();
        }
    }
}


