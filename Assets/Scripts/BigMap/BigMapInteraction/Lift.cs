
using System.Collections;
using UnityEngine;

public class Lift : InteractionObject {

    [SerializeField] private Transform TargetPosition;

    protected override void Awake() {
        this.IsBindTask = false;
        base.Awake();
    }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.电梯;
    }

    protected override void Interaction() {
        if (this.InAreaPlayer) {
            StopAllCoroutines();
            StartCoroutine(LiftCoroutine());
        }
    }

    private IEnumerator LiftCoroutine() {
        yield return SceneChangeManager.Instance.CompleteBlackScreenCoroutine(0.0f, 1.0f, () => {
            this.InAreaPlayer.transform.position = this.TargetPosition.position;
        });
    }
}


