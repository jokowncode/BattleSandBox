
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
    private int CurrentGoodsCount;
    
    public void SetContent(GoodsData data, bool canUse) {
        this.CurrentGoodsName = data.Data.GoodsName;
        this.CurrentGoodsCount = data.GoodsCount;
        
        this.GoodsNameText.text = data.Data.GoodsName;
        this.GoodsCountText.text = $"x{data.GoodsCount}";
        this.IconImage.sprite = data.Data.GoodsSprite;
        this.IconBackgroundImage.sprite = data.Data.GoodsBackgroundSprite;

        this.IconButton.enabled = canUse;
        this.IconButton.onClick.AddListener(() => {
            this.CurrentGoodsCount -= 1;
            if (this.CurrentGoodsCount <= 0) {
                Destroy(this.gameObject);
            } else {
                this.GoodsCountText.text = $"x{this.CurrentGoodsCount}";
            }
            OnClicked?.Invoke(this.CurrentGoodsName);
        });
    }
}


