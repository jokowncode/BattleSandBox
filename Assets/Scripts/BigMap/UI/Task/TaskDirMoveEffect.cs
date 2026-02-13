
using System;
using UnityEngine;

public class TaskDirMoveEffect : MonoBehaviour {

    [SerializeField] private Vector3 Dir = Vector3.right;
    [SerializeField] private float Duration = 1.0f;
    [SerializeField] private float MoveSpeed = 1.0f;

    private float t = 0.0f;
    private Vector3 StartPosition;
    
    
    private void Awake() {
        this.StartPosition = this.transform.localPosition;
        this.gameObject.SetActive(false);
    }

    private void OnEnable() {
        this.transform.localPosition = this.StartPosition;
        this.t = 0.0f;
    }

    private void Update() {
        this.transform.localPosition += this.MoveSpeed * Time.deltaTime * this.Dir;
        t += Time.deltaTime;
        if (t >= this.Duration) {
            t = 0.0f;
            this.Dir = -this.Dir;
        }
    }
}



