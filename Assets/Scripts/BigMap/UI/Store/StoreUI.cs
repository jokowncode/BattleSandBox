
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour {

    [SerializeField] private Transform GoodsContainer;
    [SerializeField] private StoreGoodsUI StoreGoodsPrefab;

    public static StoreUI Instance;
    
    private CanvasGroup StoreCanvasGroup;
    private List<StoreGoodsUI> CurrentGoods = new List<StoreGoodsUI>();

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
        UpdateStoreUI(store.Goods);
    }

    private void UpdateStoreUI(List<StoreGoodsData> goods) {
        // Clear Old Goods
        foreach (Transform oldGoods in this.GoodsContainer) {
            Destroy(oldGoods.gameObject);
        }

        this.CurrentGoods.Clear();
        // Add Current Goods
        foreach (StoreGoodsData goodsData in goods) {
            StoreGoodsUI currentGoods = Instantiate(this.StoreGoodsPrefab, this.GoodsContainer);
            currentGoods.SetData(goodsData);
            this.CurrentGoods.Add(currentGoods);
        }
    }

    public void RemoveGoods(StoreGoodsData data) {
        if (!this.CurrentStore) return;
        this.CurrentStore.RemoveGoods(data);
        UpdateStoreUI(this.CurrentStore.Goods);
    }

    public void StartInstructionMode() {
        this.IsInstructionMode = true;
        foreach (StoreGoodsUI goods in this.CurrentGoods) {
            goods.TransitionPurchase(false);
        }
    }

    public void StoreInstructionMode(int index, Action onGoodsBePurchased = null) {
        if(index < 0 || index >= this.CurrentGoods.Count) return;
        this.CurrentGoods[index].OnPurchase += () => this.IsInstructionMode = false;
        this.CurrentGoods[index].OnPurchase += onGoodsBePurchased;
        for (int i = 0; i < this.CurrentGoods.Count; i++) {
            this.CurrentGoods[i].TransitionPurchase(i == index);    
        }
    }
}


