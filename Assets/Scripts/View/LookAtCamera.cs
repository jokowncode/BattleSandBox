
using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    [SerializeField] private bool Invert = false;
    
    private Camera MainCamera;

    private void Awake() {
        MainCamera = Camera.main;
    }

    private void LateUpdate() {
        int sign = Invert ? -1 : 1;
        this.transform.forward = sign * MainCamera.transform.forward;        
    }
}


