using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter {
    protected override void Start() {
        base.Start();
        this.Move.ChangeForward(-1.0f);
        this.Move.Agent.enabled = false;
    }

    public void Deploy(Vector3 pos) {
        this.transform.position = pos;
        this.Move.Agent.enabled = true;
    }
}
