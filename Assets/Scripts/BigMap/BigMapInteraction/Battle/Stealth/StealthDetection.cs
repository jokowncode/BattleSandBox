
using System;
using System.Collections;
using UnityEngine;

public class StealthDetection : MonoBehaviour {

    [SerializeField] private bool CanMove = true;
    [SerializeField] private Transform MovePointsContainer;
    [SerializeField] private float MoveDuration = 1.0f;
    
    public Action OnDetection;

    private bool IsActivate = false;

    private void OnTriggerEnter(Collider other) {
        if (!this.IsActivate) return;
        if (!other.TryGetComponent(out Player _)) return;
        StopAllCoroutines();
        OnDetection?.Invoke();
    }

    public void Activate() {
        this.IsActivate = true;
        if (this.CanMove) {
            StartCoroutine(MoveCoroutine());
        }
    }

    public void Deactivate() {
        this.IsActivate = false;
        StopAllCoroutines();
    }

    private IEnumerator MoveCoroutine() {
        if (this.MovePointsContainer.childCount < 2) yield break;
        this.MovePointsContainer.parent = null;
        int startIndex = 0;
        int endIndex = 1;
        bool isRight = true;
        while (this.IsActivate) {
            Vector3 startPos = this.MovePointsContainer.GetChild(startIndex).position;
            Vector3 endPos = this.MovePointsContainer.GetChild(endIndex).position;
            this.transform.position = startPos;
            for (float t = 0.0f; t <= this.MoveDuration; t += Time.deltaTime) {
                this.transform.position = Vector3.Lerp(startPos, endPos, t / MoveDuration);    
                yield return null;
            }
            this.transform.position = endPos;
            startIndex = endIndex;
            endIndex = isRight ? startIndex + 1 : startIndex - 1;
            if (endIndex >= this.MovePointsContainer.childCount) {
                endIndex = startIndex - 1;
                isRight = false;
            }

            if (endIndex < 0) {
                endIndex = startIndex + 1;
                isRight = true;
            }
        }
    }
}


