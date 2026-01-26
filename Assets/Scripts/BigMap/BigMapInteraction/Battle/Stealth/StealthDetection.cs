
using System;
using System.Collections;
using UnityEngine;

public class StealthDetection : MonoBehaviour {

    [SerializeField] private bool CanMove = true;
    [SerializeField] private Transform MovePointsContainer;
    [SerializeField] private float MoveDuration = 1.0f;
    
    public Action OnDetection;

    private bool IsActivate = false;
    private bool IsStart = false;
    private int NextIndex = 0;

    private Vector3 StartPosition;

    private void Awake() {
        this.StartPosition = this.transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (!this.IsActivate) return;
        if (!other.TryGetComponent(out Player _)) return;
        StopAllCoroutines();
        OnDetection?.Invoke();
    }

    public void Activate() {
        this.IsActivate = true;
        if (this.CanMove && !this.IsStart) {
            this.IsStart = true;
            StartCoroutine(MoveCoroutine());
        }
    }

    public void Deactivate() {
        this.IsActivate = false;
        this.IsStart = false;
        StopAllCoroutines();
    }

    private Vector3 GetChildPosition(int index) {
        return index < 0 ? this.StartPosition : this.MovePointsContainer.GetChild(index).position;
    }

    private IEnumerator MoveCoroutine() {
        if (this.MovePointsContainer.childCount == 0) yield break;
        this.MovePointsContainer.parent = null;
        bool isRight = true;
        while (this.IsActivate) {
            Vector3 startPos = this.transform.position;
            Vector3 endPos = GetChildPosition(this.NextIndex);
            for (float t = 0.0f; t <= this.MoveDuration; t += Time.deltaTime) {
                this.transform.position = Vector3.Lerp(startPos, endPos, t / MoveDuration);    
                yield return null;
            }
            this.transform.position = endPos;

            NextIndex = isRight ? NextIndex + 1 : NextIndex - 1;
            if (NextIndex >= this.MovePointsContainer.childCount) {
                NextIndex -= 2;
                isRight = false;
            }

            if (NextIndex < -1) {
                NextIndex += 2;
                isRight = true;
            }
        }
    }
}


