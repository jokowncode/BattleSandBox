
using System;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAnimationUI : MonoBehaviour {

    [SerializeField] private float Interval = 0.1f;
    [SerializeField] private Image ImageUI;
    
    private Sprite[] Anims;
    private float T = 0.0f;
    private int CurrentIndex = 0;

    private void Update() {
        if (Anims == null || Anims.Length == 0) return;
        if (Time.time - T < Interval) {
            return;
        }

        CurrentIndex += 1;
        ImageUI.sprite = Anims[CurrentIndex % Anims.Length];
        T = Time.time;
    }

    public void SetAnims(Sprite[] images) {
        this.Anims = images;
        this.CurrentIndex = 0;
        T = Time.time;
    }

    public void ResetAnim() {
        this.CurrentIndex = 0;
        ImageUI.sprite = Anims[this.CurrentIndex];
        T = Time.time;
    }
}


