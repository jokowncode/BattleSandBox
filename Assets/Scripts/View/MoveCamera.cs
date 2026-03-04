
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    [SerializeField] private float Duration = 1.0f;

    public Action OnArrive;
    private float StartX;

    private void Awake() {
        StartX = transform.position.x;
    }

    public void GoBackToStartX() {
        Vector3 pos = this.transform.position;
        pos.x = this.StartX;
        this.transform.position = pos;
    }

    public void MoveToInXDirByOffset(float offset) {
        this.MoveToInXDir(this.transform.position.x + offset - this.StartX);
    }

    public void MoveToInXDir(float x) {
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine(x));
    }

    private IEnumerator MoveCoroutine(float offset) {
        float destX = StartX + offset;
        
        float moveStartX = transform.position.x;
        Vector3 pos = transform.position;
        
        for (float t = 0.0f; t < Duration; t += Time.deltaTime) {
            float currentX = Mathf.Lerp(moveStartX, destX, t / Duration);
            pos.x = currentX;
            transform.position = pos;
            yield return null;
        }

        pos.x = destX;
        transform.position = pos;
        OnArrive?.Invoke();
    }
}

