
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
        
        // TODO: Player Current Money is Enough To Buy This Goods And Minus Money
        this.PurchaseButton.onClick.AddListener(() => {
            switch (data.Type) {
                case GoodsType.Hero:
                    HeroWarehouseManager.Instance.AddHero(data.GoodsName);
                    break;
                case GoodsType.PassiveEntry:
                    PassiveEntryWarehouseManager.Instance.AddPassiveEntry(data.GoodsName);
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

