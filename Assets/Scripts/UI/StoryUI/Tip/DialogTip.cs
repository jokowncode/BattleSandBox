
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogTip : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI TipText;
    [SerializeField] private float Duration = 0.5f;
    [SerializeField] private float Speed = 2.0f;
    
    private RectTransform TipTrans;

    private void Awake() {
        this.TipTrans = this.GetComponent<RectTransform>();
    }

    public void Show(string text) {
        StartCoroutine(ShowCoroutine(text));
    }

    private IEnumerator ShowCoroutine(string text) {
        this.TipText.text = text;
        Vector2 size = this.TipTrans.sizeDelta; 
        size.x = text.Length * this.TipText.fontSize + 100.0f;
        this.TipTrans.sizeDelta = size;

        for (float t = 0; t <= this.Duration; t += Time.deltaTime) {
            Vector3 pos = this.transform.position;
            pos += this.Speed * t * Vector3.up;
            this.transform.position = pos;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}


