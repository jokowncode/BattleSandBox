
using System.Collections;
using UnityEngine;

public class StealthBattleRoom : BattleRoom {

    [SerializeField] private Transform LeftPoint;
    [SerializeField] private Transform RightPoint;
    [SerializeField] private StealthDetection[] Detectors;

    private float CurrentPlayerDir;

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
            this.InAreaPlayer.transform.position = this.CurrentPlayerDir < 0.0f ?
                this.RightPoint.position : this.LeftPoint.position;
        }
    }

    protected override void PlayerEnter() {
        if (PlayerPrefs.HasKey(GetName())) {
            this.CurrentPlayerDir = PlayerPrefs.GetFloat(GetName());
        } else {
            this.CurrentPlayerDir = Mathf.Sign(this.InAreaPlayer.Move.HorizontalDir.x);
            PlayerPrefs.SetFloat(GetName(), this.CurrentPlayerDir);
        }
        
        base.PlayerEnter();
        this.EnableInteraction(false);
        foreach (StealthDetection detector in this.Detectors) {
            detector.Activate();
        }
    }

    protected override void OnTriggerExit(Collider other) {
        base.OnTriggerExit(other);
        PlayerPrefs.DeleteKey(GetName());
        foreach (StealthDetection detector in this.Detectors) {
            detector.Deactivate();
        }
    }
}



