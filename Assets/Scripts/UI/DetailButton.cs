
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailButton : MonoBehaviour {

    [SerializeField] private Sprite DefaultOuterIcon;
    [SerializeField] private Image OuterIconImage;
    [SerializeField] private Button UseButton;
    [SerializeField] protected TextMeshProUGUI DescText;
    [SerializeField] protected TextMeshProUGUI NameText;
    [SerializeField] protected TextMeshProUGUI CountText;

    public Action<string> OnButtonClicked;
    private Image InnerIconImage;
    
    private void Awake() {
        this.InnerIconImage = this.UseButton.GetComponent<Image>();
    }

    private void SetIcon(Sprite innerIcon, Sprite outerIcon) {
        if (this.InnerIconImage) {
            this.InnerIconImage.color = new Color(1.0f, 1.0f, 1.0f, innerIcon ? 1.0f : 0.0f);
            this.InnerIconImage.sprite = innerIcon;
        }

        if (this.OuterIconImage) {
            this.OuterIconImage.sprite = outerIcon ? outerIcon : this.DefaultOuterIcon;
        }
    }

    public void SetData(string desc, string showName, int count, bool canUse) {
        this.DescText.text = desc;
        this.CountText.text = count.ToString("D3");
        this.NameText.text = showName;
        
        StoreGoodsData data = GoodsWarehouseManager.Instance.GetGoodsData(showName);            
        if(data) this.SetIcon(data.GoodsSprite, data.GoodsBackgroundSprite);
        
        this.UseButton.enabled = canUse && count != 0;
        if (!canUse) return;
        this.UseButton.onClick.AddListener(() => OnButtonClicked?.Invoke(this.NameText.text));
    }
}


