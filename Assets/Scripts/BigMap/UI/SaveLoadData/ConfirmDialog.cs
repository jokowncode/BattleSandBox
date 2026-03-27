
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDialog : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI TipText;
    
    private Action OnConfirm;
    
    public void Cancel() {
        this.gameObject.SetActive(false);
        this.OnConfirm = null;
    }

    public void Confirm() {
        this.OnConfirm?.Invoke();
        this.Cancel();
    }

    public void Show(Action onConfirm, string text) {
        this.gameObject.SetActive(true);
        this.OnConfirm = onConfirm;
        this.TipText.text = text;
    }
}


