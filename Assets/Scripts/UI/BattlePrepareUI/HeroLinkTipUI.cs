
using System;
using UnityEngine;

public class HeroLinkTipUI : MonoBehaviour {
    
    [SerializeField] private RectTransform LeftContainer;
    [SerializeField] private RectTransform RightContainer;
    [SerializeField] private RectTransform LinkLinePrefab;
    [SerializeField] private float LineMargin = 14.0f;

    [SerializeField] private RectTransform LeftVerticalLine;
    [SerializeField] private RectTransform RightVerticalLine;

    private RectTransform Rect;
    
    private void Awake() {
        this.Rect = this.GetComponent<RectTransform>();
        Hide();
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

    public void Show(float lCenter, float rCenter, float y) {
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
        this.Rect.position = new Vector2(middle, y + 115.0f);
        
        // TODO: ACCURATE CALC (MAYBE REASON: Screen Point diff UI Point)
        float distance = 4 * this.LineMargin * (rCenter - lCenter) / 154.0f;
        float halfW = this.LinkLinePrefab.sizeDelta.x / 2f;
        for (float l = 0.0f; l * this.LineMargin + halfW <= distance; l+=1.0f) {
            RectTransform leftTrans = Instantiate(LinkLinePrefab, LeftContainer);
            leftTrans.localPosition = new Vector3(-(l+1)*this.LineMargin, 0.0f, 0.0f);
            
            RectTransform rightTrans = Instantiate(LinkLinePrefab, RightContainer);
            rightTrans.localPosition = new Vector3((l+1)*this.LineMargin, 0.0f, 0.0f);
        }

        LeftVerticalLine.position = new Vector3(lCenter, y + 90.0f, 0.0f);
        RightVerticalLine.position = new Vector3(rCenter, y + 90.0f, 0.0f);
        this.gameObject.SetActive(true);
    }
}


