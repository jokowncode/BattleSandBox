
using System;
using TMPro;
using UnityEngine;

public class InteractionTip : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI TipText;
    [SerializeField] private TextMeshProUGUI EText;
    [SerializeField] private AnimationCurve Curve;

    private Animator TipAnimator;

    private void Awake() {
        this.TipAnimator = this.GetComponent<Animator>();
    }

    public void Show(string text, bool canInteract) {
        this.TipText.text = text;
        this.TipText.characterSpacing = Curve.Evaluate(text.Length);
        this.TipText.gameObject.SetActive(false);
        this.EText.text = canInteract ? "E" : "×";
        this.TipAnimator.SetTrigger(AnimationParams.Show);
    }

    public void Hide() {
        this.TipAnimator.SetTrigger(AnimationParams.Hide);
    }
}


