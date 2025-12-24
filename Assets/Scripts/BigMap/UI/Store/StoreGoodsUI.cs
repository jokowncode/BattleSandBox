
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
        this.MoneyText.text = data.Money.ToString();
        this.GoodsImage.sprite = data.GoodsSprite;
        this.GoodsOutline.effectColor = data.GoodsColor;
        
        this.PurchaseButton.onClick.AddListener(() => {

            float currentMoney = GameManager.Instance.Money;
            if (currentMoney < data.Money) {
                return;
            }
            GameManager.Instance.SetMoney(currentMoney - data.Money);

            switch (data.Type) {
                case GoodsType.Hero:
                    HeroWarehouseManager.Instance.AddHero(data.GoodsName);
                    break;
                case GoodsType.PassiveEntry:
                    PassiveEntryWarehouseManager.Instance.AddPassiveEntry(data.GoodsName, 1);
                    break;
                case GoodsType.EXP:
                    // Debug.Log("Buy EXP");
                    break;
            }
            
            // Destroy Goods If Purchase Success
            StoreUI.Instance.RemoveGoods(data);
            OnPurchase?.Invoke();
        });
    }

    public void TransitionPurchase(bool canPurchase) {
        this.PurchaseButton.interactable = canPurchase;
    }
}

