
using UnityEngine;

public enum ObstacleType {
    Inside,
    Outside
}

public class Obstacle : InteractionObject {

    [SerializeField] private ObstacleType Type = ObstacleType.Outside;
    [SerializeField] private InteractionObject CancelObstacleObj;
    [SerializeField] private bool ShowTip = false;
    
    private BoxCollider ObstacleCollider;

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.障碍;
    }

    protected override void Awake() {
        base.Awake();
        ObstacleCollider = this.GetComponent<BoxCollider>();
    }

    protected override void LoadBigMapData() {
        if (!this.IsEnd && this.CancelObstacleObj) {
            this.CancelObstacleObj.OnInteractionEnded += () => {
                if(!this.IsEnd) this.EndInteraction();
            };
        }
        
        // TODO: Obstacle Look Change
    }

    protected override void Interaction() { }

    protected override void Update() {
        if (!this.IsEnd && Type == ObstacleType.Inside 
        && this.ObstacleCollider.bounds.Contains(this.InAreaPlayer.transform.position)){
            this.InAreaPlayer.SetCollider(this.ObstacleCollider, PlayerInAreaColliderDir.Both);
        } 
    }

    protected override void PlayerEnter() {
        this.InAreaPlayer.TransitionInteractionTip(this.ShowTip && !this.IsEnd, this.InteractionObjShowName, false);
        this.enabled = this.Type == ObstacleType.Inside;
        if (!this.IsEnd) {
            if (Type == ObstacleType.Outside) {
                if (this.InAreaPlayer.transform.position.x < this.transform.position.x) {
                    this.InAreaPlayer.SetCollider(this.ObstacleCollider, PlayerInAreaColliderDir.Left);
                } else {
                    this.InAreaPlayer.SetCollider(this.ObstacleCollider, PlayerInAreaColliderDir.Right);
                }    
            }
        }
    }

    protected override void OnTriggerExit(Collider other) {
        if (!other.TryGetComponent(out Player _)) return;
        if (this.InAreaPlayer) {
            this.InAreaPlayer.TransitionInteractionTip(false, this.InteractionObjShowName, false);
            this.InAreaPlayer.SetCollider(null);
            this.InAreaPlayer = null;
            this.enabled = false;
        }
    }
}

