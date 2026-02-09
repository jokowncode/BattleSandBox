
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreGoodsUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI MoneyText;
    [SerializeField] private Image GoodsImage;
    [SerializeField] private Button PurchaseButton;

    private Outline GoodsOutline;

    public Action OnPurchase;

    private void Awake() {
        this.GoodsOutline = this.GetComponent<Outline>();
    }

    public void SetData(StoreGoodsData data) {
        if (data.Type == GoodsType.None) return;
        this.MoneyText.text = data.Money.ToString();

        if (data.Type == GoodsType.角色) {
            // TODO: 单独处理Hero的Image
        }

        GoodsImageData imageData = GoodsWarehouseManager.Instance.GetImageData(data.Type);
        this.GoodsImage.sprite = imageData ? imageData.IconSprite : null;
        this.GoodsOutline.effectColor = data.GoodsColor;
        
        this.PurchaseButton.onClick.AddListener(() => {

            float currentMoney = GameManager.Instance.Money;
            if (currentMoney < data.Money) {
                return;
            }

            if (!GoodsWarehouseManager.Instance.AddGoods(data)) return;
            GameManager.Instance.SetMoney(currentMoney - data.Money);
            // Destroy Goods If Purchase Success
            StoreUI.Instance.RemoveGoods(data);
            OnPurchase?.Invoke();
        });
    }

    public void TransitionPurchase(bool canPurchase) {
        this.PurchaseButton.interactable = canPurchase;
    }
}

