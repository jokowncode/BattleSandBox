using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter {
    protected override void Start() {
        base.Start();
        this.Move.ChangeForward(-1.0f);
    }
}
