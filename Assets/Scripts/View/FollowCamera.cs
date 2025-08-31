
using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    private float StartX;

    private void Awake() {
        StartX = this.transform.position.x;
    }

    private void LateUpdate() {
        
    }
}

