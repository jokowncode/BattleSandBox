
using System;
using UnityEngine;

public class TestDialog : MonoBehaviour {

    [SerializeField] private DialogGraph Dialog;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DialogManager.Instance.PlayNewDialog(this.Dialog);
            this.enabled = false;
        }
    }
}



