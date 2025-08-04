using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter {
    private void Start() {
        this.Move.ChangeForward(-1.0f);
    }
}
