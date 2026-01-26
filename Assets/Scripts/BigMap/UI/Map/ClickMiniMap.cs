
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickMiniMap : MonoBehaviour, IPointerClickHandler {

    [SerializeField] private TotalMapUI TotalMap;

    private RawImage MiniMapImage;
    
    private void Awake() {
        this.MiniMapImage = this.GetComponent<RawImage>();
        if (this.TotalMap) {
            this.TotalMap.OnClose += () => {
                this.MiniMapImage.enabled = true;
            };
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.clickCount < 2) return;
        this.MiniMapImage.enabled = false;
        this.TotalMap.Show();
    }
}


