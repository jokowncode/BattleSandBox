
using System;
using UnityEngine;

public class EdgeManager : MonoBehaviour {

    public static EdgeManager Instance;

    public float LeftEdgeX { get; private set; }
    public float RightEdgeX { get; private set; }

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        foreach (Transform child in this.transform) {
            this.LeftEdgeX = Mathf.Min(child.position.x, this.LeftEdgeX);
            this.RightEdgeX = Mathf.Max(child.position.x, this.RightEdgeX);
        }
    }
}

