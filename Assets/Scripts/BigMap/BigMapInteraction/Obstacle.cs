
using UnityEngine;

public class Obstacle : InteractionObject {

    [SerializeField] private InteractionObject CancelObstacleObj;
    
    private BoxCollider ObstacleCollider;

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

    protected override string GetName() {
        Vector3 pos = this.transform.position;
        return $"Obstacle_{pos.x}_{pos.y}_{pos.z}";
    }

    protected override void Interaction() { }

    protected override void PlayerEnter() {
        this.EnableInteraction(false);
        if (!this.IsEnd) {
            if (this.InAreaPlayer.transform.position.x < this.transform.position.x) {
                this.InAreaPlayer.SetCollider(this.ObstacleCollider, PlayerInAreaColliderDir.Left);
            } else {
                this.InAreaPlayer.SetCollider(this.ObstacleCollider, PlayerInAreaColliderDir.Right);
            }
        }
    }

    protected override void OnTriggerExit(Collider other) {
        if (!other.TryGetComponent(out Player _)) return;
        this.InAreaPlayer.SetCollider(null);
        this.InAreaPlayer = null;
    }
}

