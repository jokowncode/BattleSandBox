
using System;
using UnityEngine;

public class ScrollContentAutoAdaptHeight : MonoBehaviour {
    
    private RectTransform RectTrans;
    
    private void Awake() {
        this.RectTrans = this.GetComponent<RectTransform>();
    }

    private void LateUpdate() {
        Vector2 currentSize = this.RectTrans.sizeDelta;
        currentSize.y = this.transform.childCount * 100.0f;
        this.RectTrans.sizeDelta = currentSize;
    }
}



