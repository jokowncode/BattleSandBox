
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour {

    [SerializeField] private List<StoreGoodsData> AllStoreGoods;
    [SerializeField] private Transform GoodsContainer;
    [SerializeField] private StoreGoodsUI StoreGoodsPrefab;

    public static StoreUI Instance;
    
    private CanvasGroup StoreCanvasGroup;
    private List<StoreGoodsUI> CurrentGoods = new List<StoreGoodsUI>();

    private Store CurrentStore;
    private bool IsInstructionMode = false;
    
    private Dictionary<string, StoreGoodsData> AllStoreGoodsMap;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        this.StoreCanvasGroup = this.GetComponent<CanvasGroup>();

        this.AllStoreGoodsMap = new Dictionary<string, StoreGoodsData>();
        foreach (StoreGoodsData storeGoodsData in AllStoreGoods) {
            this.AllStoreGoodsMap.Add(storeGoodsData.GoodsName, storeGoodsData);
        }
    }

    public List<string> RandomGoodsSimple() {
        List<string> result = new List<string>();
        foreach (StoreGoodsData data in AllStoreGoods) {
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
        UpdateStoreUI(store.Goods);
    }

    private void UpdateStoreUI(List<string> goods) {
        // Clear Old Goods
        foreach (Transform oldGoods in this.GoodsContainer) {
            Destroy(oldGoods.gameObject);
        }

        this.CurrentGoods.Clear();
        // Add Current Goods
        foreach (string goodsName in goods) {
            if (!this.AllStoreGoodsMap.ContainsKey(goodsName)) continue;
            StoreGoodsUI currentGoods = Instantiate(this.StoreGoodsPrefab, this.GoodsContainer);
            currentGoods.SetData(this.AllStoreGoodsMap[goodsName]);
            this.CurrentGoods.Add(currentGoods);
        }
    }

    public void RemoveGoods(StoreGoodsData data) {
        if (!this.CurrentStore) return;
        this.CurrentStore.RemoveGoods(data.GoodsName);
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


