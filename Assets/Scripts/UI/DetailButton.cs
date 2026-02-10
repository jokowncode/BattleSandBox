
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailButton : MonoBehaviour {

    [SerializeField] private Sprite DefaultOuterIcon;
    [SerializeField] private Image InnerIconImage;
    [SerializeField] private Image OuterIconImage;
    [SerializeField] private Button UseButton;
    [SerializeField] protected TextMeshProUGUI DescText;
    [SerializeField] protected TextMeshProUGUI NameText;
    [SerializeField] protected TextMeshProUGUI CountText;

    public Action<string, int> OnButtonClicked;

    public string Name => this.NameText.text;

    public int GetCurrentCount() {
        if (!this.CountText) return 0;
        int.TryParse(this.CountText.text, out int count);
        return count;
    }

    private void Awake() {
        this.UseButton.onClick.AddListener(() => {
            OnButtonClicked?.Invoke(this.NameText.text, GetCurrentCount());
        });
    }

    public void SetIcon(Sprite innerIcon, Sprite outerIcon) {
        if (this.InnerIconImage) {
            this.InnerIconImage.color = new Color(1.0f, 1.0f, 1.0f, innerIcon ? 1.0f : 0.0f);
            this.InnerIconImage.sprite = innerIcon;
        }

        if (this.OuterIconImage) {
            this.OuterIconImage.sprite = outerIcon ? outerIcon : this.DefaultOuterIcon;
        }
    }

    public void SetCount(int newCount) {
        if (newCount <= 0) {
            Destroy(this.gameObject);
            return;
        }
        this.CountText.text = newCount.ToString("D3");
    }

    public void SetData(string desc, string showName, int count, bool canUse, GoodsType type) {
        if (this.DescText) this.DescText.text = desc;
        if (this.CountText) this.CountText.text = count.ToString("D3");
        this.NameText.text = showName;
        
        GoodsImageData data = GoodsWarehouseManager.Instance.GetImageData(type);         
        this.SetIcon(data ? data.IconSprite : null, data ? data.BorderSprite : null);
        this.UseButton.enabled = canUse && count != 0;
    }

    public void TransitionButtonInteractable(bool enable) {
        this.UseButton.interactable = enable;
    }
}


