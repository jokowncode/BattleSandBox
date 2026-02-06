
using System;
using UnityEngine;

public class HeroLinkTipUI : MonoBehaviour {
    
    [SerializeField] private RectTransform LeftContainer;
    [SerializeField] private RectTransform RightContainer;
    [SerializeField] private RectTransform LinkLinePrefab;
    [SerializeField] private float LineMargin = 14.0f;

    [SerializeField] private RectTransform LeftVerticalLine;
    [SerializeField] private RectTransform RightVerticalLine;

    [SerializeField] private float LineHeight = 40.0f;

    private RectTransform Rect;
    
    private void Awake() {
        this.Rect = this.GetComponent<RectTransform>();
        Hide();
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

    public void Show(float lCenter, float rCenter, float y, float height) {
        foreach (Transform child in LeftContainer) {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in RightContainer) {
            Destroy(child.gameObject);
        }

        if (lCenter > rCenter) {
            (lCenter, rCenter) = (rCenter, lCenter);
        }

        float middle = (lCenter + rCenter) / 2f;
        this.Rect.localPosition = new Vector2(middle, y + height / 2.0f + this.LineHeight);
        
        float distance = Mathf.Abs(middle - lCenter) - Mathf.Abs(this.LeftContainer.localPosition.x);
        float halfW = this.LinkLinePrefab.sizeDelta.x / 2f;
        float l = 0.0f;
        for (; l * this.LineMargin + halfW <= distance; l+=1.0f) {
            RectTransform leftTrans = Instantiate(LinkLinePrefab, LeftContainer);
            leftTrans.localPosition = new Vector3(-(l+1)*this.LineMargin, 0.0f, 0.0f);
            
            RectTransform rightTrans = Instantiate(LinkLinePrefab, RightContainer);
            rightTrans.localPosition = new Vector3((l+1)*this.LineMargin, 0.0f, 0.0f);
        }

        float lineX = l * this.LineMargin + halfW + Mathf.Abs(this.LeftContainer.localPosition.x);
        float lineY = this.LineHeight - this.LineMargin;
        LeftVerticalLine.localPosition = new Vector3(-lineX, -lineY, 0.0f);
        RightVerticalLine.localPosition = new Vector3(lineX, -lineY, 0.0f);
        this.gameObject.SetActive(true);
    }
}


