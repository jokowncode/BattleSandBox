
using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    [SerializeField] private Transform Target;

    private Vector3 Offset;
    
    private void Awake() {
        if (Target) {
            this.Offset = this.transform.position - Target.transform.position;
        }
    }

    private void LateUpdate() {
        if (!this.Target) return;
        this.transform.position = this.Target.transform.position + this.Offset;
    }
}

