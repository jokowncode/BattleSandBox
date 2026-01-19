
using System.Collections;
using UnityEngine;

public class StealthBattleRoom : BattleRoom {

    [SerializeField] private Transform StartPoint;
    [SerializeField] private Transform SuccessPoint;
    [SerializeField] private StealthDetection[] Detectors;

    protected override void Awake() {
        base.Awake();
        this.IsEndCanInteract = true;
        foreach (StealthDetection detector in this.Detectors) {
            detector.OnDetection += () => {
                OnInteractionPre?.Invoke();
                base.Interaction();
            };
        }

        this.OnDefeat += GoToStartPoint;
        this.OnVictory += GoToStartPoint;
    }

    private void GoToStartPoint() {
        if (this.InAreaPlayer) {
            this.InAreaPlayer.transform.position = this.StartPoint.position;
        }
    }

    protected override void Update() {
        if (!this.InAreaPlayer) return;
        if (this.Collider.bounds.Contains(this.InAreaPlayer.transform.position)){
            this.InAreaPlayer.SetCollider(this.Collider);
        }

        if (Vector3.SqrMagnitude(this.InAreaPlayer.transform.position - this.SuccessPoint.position) < 0.1f) {
            this.EndInteraction();
            this.enabled = false;
            this.InAreaPlayer.SetCollider(null);
        }
    }

    protected override void PlayerEnter() {
        base.PlayerEnter();
        this.EnableInteraction(false);
        this.enabled = true;
        foreach (StealthDetection detector in this.Detectors) {
            detector.Activate();
        }
    }

    protected override void OnTriggerExit(Collider other) {
        base.OnTriggerExit(other);
        foreach (StealthDetection detector in this.Detectors) {
            detector.Deactivate();
        }
    }
}



