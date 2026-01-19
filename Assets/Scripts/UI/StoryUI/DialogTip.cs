
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogTip : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI TipText;
    [SerializeField] private float WaitTime = 0.5f;
    [SerializeField] private float Duration = 0.5f;
    
    private CanvasGroup TipCanvasGroup;
    private WaitForSeconds WaitTimer;
    private RectTransform TipTrans;

    private void Awake() {
        this.TipTrans = this.GetComponent<RectTransform>();
        this.TipCanvasGroup = this.GetComponent<CanvasGroup>();
        this.TipCanvasGroup.alpha = 0.0f;
        this.WaitTimer = new WaitForSeconds(this.WaitTime);
    }

    public void Show(string text) {
        StopAllCoroutines();
        StartCoroutine(ShowCoroutine(text));
    }

    private IEnumerator ShowCoroutine(string text) {
        this.TipText.text = text;
        Vector2 size = this.TipTrans.sizeDelta; 
        size.x = text.Length * this.TipText.fontSize;
        this.TipTrans.sizeDelta = size;
        
        yield return ChangeAlphaCoroutine(0.0f, 1.0f);
        yield return this.WaitTimer;
        yield return ChangeAlphaCoroutine(1.0f, 0.0f);
    }

    private IEnumerator ChangeAlphaCoroutine(float start, float end) {
        this.TipCanvasGroup.alpha = start;
        for (float t = 0.0f; t <= this.Duration; t += Time.deltaTime) {
            this.TipCanvasGroup.alpha = Mathf.Lerp(start, end, t / this.Duration);
            yield return null;
        }
        this.TipCanvasGroup.alpha = end;
    }
}


