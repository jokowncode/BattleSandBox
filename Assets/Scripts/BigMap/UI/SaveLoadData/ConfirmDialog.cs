
using System;
using UnityEngine;

public class ConfirmDialog : MonoBehaviour {

    private Action OnConfirm;
    
    public void Cancel() {
        this.gameObject.SetActive(false);
        this.OnConfirm = null;
    }

    public void Confirm() {
        this.OnConfirm?.Invoke();
        this.Cancel();
    }

    public void Show(Action onConfirm) {
        this.gameObject.SetActive(true);
        this.OnConfirm = onConfirm;
    }
}


