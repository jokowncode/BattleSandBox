
using System;
using TMPro;
using UnityEngine;

public class InteractionTip : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI TipText;
    [SerializeField] private TextMeshProUGUI EText;
    [SerializeField] private AnimationCurve Curve;

    private Animator TipAnimator;
    private CanvasGroup TipCanvasGroup;

    private void Awake() {
        this.TipAnimator = this.GetComponent<Animator>();
        this.TipCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    public void Show(string text, bool canInteract) {
        this.TipText.text = text;
        this.TipText.characterSpacing = Curve.Evaluate(text.Length);
        this.TipText.gameObject.SetActive(false);
        this.EText.text = canInteract ? "E" : "×";
        this.TipAnimator.SetBool(AnimationParams.Show, true);
    }

    public void Hide() {
        if (this.TipCanvasGroup.alpha < 0.1f) return;
        this.TipAnimator.SetBool(AnimationParams.Show, false);
    }
}


