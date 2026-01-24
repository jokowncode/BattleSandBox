

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTipContainer : MonoBehaviour {
    
    [SerializeField] private DialogTip DialogTipPrefab;
    [SerializeField] private float WaitTime = 0.8f;

    private Queue<string> TipTexts = new ();
    private float LastShowTime = -1.0f;

    private void Awake() {
        this.enabled = false;
    }

    public void StartTip() {
        if (this.enabled) return;
        this.enabled = true;
        this.LastShowTime = -1.0f;

        foreach (Transform child in this.transform) {
            Destroy(child.gameObject);
        }
    }

    public void EndTip() {
        this.TipTexts.Clear();
        this.enabled = false;
    }

    public void AddTip(string tipText) {
        this.TipTexts.Enqueue(tipText);
    }

    private void Update() {
        if (this.TipTexts.Count == 0) return;
        if (this.LastShowTime >= 0.0f && Time.time - this.LastShowTime < this.WaitTime) return;
        DialogTip tip = Instantiate(this.DialogTipPrefab, this.transform);
        tip.Show(this.TipTexts.Dequeue());
        this.LastShowTime = Time.time;
    }
}

