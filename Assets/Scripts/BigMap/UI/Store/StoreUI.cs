
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI MoneyText;
    [SerializeField] private Transform NormalStoreGoodsContainer;
    [SerializeField] private Transform HasDescStoreGoodsContainer;
    [SerializeField] private DetailButton NormalStoreGoodsPrefab;
    [SerializeField] private DetailButton HasDescStoreGoodsPrefab;
    
    public static StoreUI Instance;
    
    private CanvasGroup StoreCanvasGroup;
    private List<DetailButton> CurrentGoods = new ();

    private Store CurrentStore;
    private bool IsInstructionMode = false;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        this.StoreCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    public List<string> RandomGoodsSimple() {
        List<string> result = new List<string>();
        foreach (StoreGoodsData data in GoodsWarehouseManager.Instance.AllGoodsData) {
            result.Add(data.GoodsName);
        }
        return result;
    }

    public void HideStoreUI() {
        if (this.IsInstructionMode) return;
        this.CurrentStore = null;
        StoreCanvasGroup.alpha = 0.0f;
        StoreCanvasGroup.interactable = false;
        StoreCanvasGroup.blocksRaycasts = false;
    }

    public void ShowStoreUI(Store store) {
        if (StoreCanvasGroup.alpha >= 0.9f) return;
        this.CurrentStore = store;
        StoreCanvasGroup.alpha = 1.0f;
        StoreCanvasGroup.interactable = true;
        StoreCanvasGroup.blocksRaycasts = true;
        this.MoneyText.text = GameManager.Instance.Money.ToString();
        UpdateStoreUI(store.CurrentGoods);
    }
    
    

    private void UpdateStoreUI(List<string> goods) {
        // Clear Old Goods
        foreach (Transform oldGoods in this.NormalStoreGoodsContainer) {
            Destroy(oldGoods.gameObject);
        }
        
        foreach (Transform oldGoods in this.HasDescStoreGoodsContainer) {
            Destroy(oldGoods.gameObject);
        }
        this.CurrentGoods.Clear();
        
        // Add Current Goods
        foreach (string goodsName in goods) {
            StoreGoodsData data = GoodsWarehouseManager.Instance.GetGoodsData(goodsName);
            if (!data || data.Type == GoodsType.None) continue;
            DetailButton button = null;
            string desc = "";
            if (data.Type is GoodsType.战术 or GoodsType.普通词条 or GoodsType.特殊词条) {
                button = Instantiate(this.HasDescStoreGoodsPrefab, HasDescStoreGoodsContainer);
                if (data.Type == GoodsType.战术 &&
                    Enum.TryParse(data.GoodsName, true, out BattleTacticType type)) {
                    desc = BattleTacticFactory.GetBattleTacticDescription(type);
                } else {
                    PassiveEntry entry = PassiveEntryWarehouseManager.Instance.GetPassiveEntryByName(data.GoodsName);
                    if (entry) desc = entry.Data.Description;
                }
            } else {
                button = Instantiate(this.NormalStoreGoodsPrefab, NormalStoreGoodsContainer);    
            }
            button.SetData(desc, data.GoodsShowName, data.Money, true, data.Type, data.GoodsName);
            if (data.Type == GoodsType.角色) {
                Hero hero = HeroWarehouseManager.Instance.GetHeroByRef(data.GoodsName);
                if (hero) {
                    button.SetIcon(hero.WarehouseData.AvatarSprite, 
                        HeroWarehouseManager.Instance.GetHeroBorderSprite(hero.Type));
                }
            }

            button.OnButtonClicked += OnGoodsClicked;
            this.CurrentGoods.Add(button);
        }
    }

    private void OnGoodsClicked(string gName, int money) {
        float currentMoney = GameManager.Instance.Money;
        if (currentMoney < money) {
            AudioManager.Instance.PlayErrorSfx();
            return;
        }
        
        StoreGoodsData gData = GoodsWarehouseManager.Instance.GetGoodsData(gName);
        if (!gData) return;
        if (!GoodsWarehouseManager.Instance.AddGoods(gData)) return;
        GameManager.Instance.SetMoney(currentMoney - money);
        this.MoneyText.text = GameManager.Instance.Money.ToString();
        RemoveGoods(gData);
    }

    private void RemoveGoods(StoreGoodsData data) {
        if (!this.CurrentStore) return;
        this.CurrentStore.RemoveGoods(data.GoodsName);
        UpdateStoreUI(this.CurrentStore.CurrentGoods);
    }

    public void StartInstructionMode() {
        this.IsInstructionMode = true;
        foreach (DetailButton goods in this.CurrentGoods) {
            goods.TransitionButtonInteractable(false);
        }
    }

    public void StoreInstructionMode(int index, Action onGoodsBePurchased = null) {
        if(index < 0 || index >= this.CurrentGoods.Count) return;
        this.CurrentGoods[index].OnButtonClicked += (_, _) => this.IsInstructionMode = false;
        this.CurrentGoods[index].OnButtonClicked += (_, _) => onGoodsBePurchased?.Invoke();
        for (int i = 0; i < this.CurrentGoods.Count; i++) {
            this.CurrentGoods[i].TransitionButtonInteractable(i == index);    
        }
    }
}


