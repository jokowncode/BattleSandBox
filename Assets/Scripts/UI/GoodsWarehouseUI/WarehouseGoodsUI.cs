
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseGoodsUI : MonoBehaviour {

    [SerializeField] private Image IconBackgroundImage;
    [SerializeField] private Image IconImage;
    [SerializeField] private Button IconButton;
    [SerializeField] private TextMeshProUGUI GoodsNameText;
    [SerializeField] private TextMeshProUGUI GoodsCountText;

    public Action<string> OnClicked;
    private string CurrentGoodsName;
    
    public void SetContent(GoodsData data, bool canUse) {
        this.CurrentGoodsName = data.Data.GoodsName;
        
        this.GoodsNameText.text = data.Data.GoodsName;
        this.GoodsCountText.text = $"x{data.GoodsCount}";
        this.IconImage.sprite = data.Data.GoodsSprite;
        this.IconBackgroundImage.sprite = data.Data.GoodsBackgroundSprite;

        this.IconButton.enabled = canUse;
        this.IconButton.onClick.AddListener(() => {
            OnClicked?.Invoke(this.CurrentGoodsName);
            int currentCount = GoodsWarehouseManager.Instance.GetGoodsCount(this.CurrentGoodsName);
            if (currentCount <= 0) {
                Destroy(this.gameObject);
            } else {
                this.GoodsCountText.text = $"x{currentCount}";
            }
        });
    }
}


