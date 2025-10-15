
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour {

    [SerializeField] private Transform GoodsContainer;
    [SerializeField] private StoreGoodsUI StoreGoodsPrefab;
    
    private CanvasGroup StoreCanvasGroup;

    private void Awake() {
        this.StoreCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    public void HideStoreUI(){
        StoreCanvasGroup.alpha = 0.0f;
        StoreCanvasGroup.interactable = false;
        StoreCanvasGroup.blocksRaycasts = false;
    }

    public void ShowStoreUI(List<StoreGoodsData> goods){
        StoreCanvasGroup.alpha = 1.0f;
        StoreCanvasGroup.interactable = true;
        StoreCanvasGroup.blocksRaycasts = true;
        UpdateStoreUI(goods);
    }

    public void UpdateStoreUI(List<StoreGoodsData> goods) {
        // Clear Old Goods
        foreach (Transform oldGoods in this.GoodsContainer) {
            Destroy(oldGoods.gameObject);
        }
        
        // Add Current Goods
        foreach (StoreGoodsData goodsData in goods) {
            StoreGoodsUI currentGoods = Instantiate(this.StoreGoodsPrefab, this.GoodsContainer);
            currentGoods.SetData(goodsData);
        }
    }
}


