

using System;
using System.Collections.Generic;
using UnityEngine;

public class GoodsWarehousePanel : MonoBehaviour {

    [SerializeField] private Transform GoodsContainer;
    [SerializeField] private WarehouseGoodsUI GoodsPrefab;
    
    public Action<string> OnClickGoods;
    private GoodsType CurrentShowGoodsType = GoodsType.None;
    
    public void Show(GoodsType type, bool canUse) {
        if (type != this.CurrentShowGoodsType) {
            this.CurrentShowGoodsType = type;
            foreach (Transform child in GoodsContainer) {
                Destroy(child.gameObject);
            }

            List<GoodsData> goods = GoodsWarehouseManager.Instance.GetGoodsByType(type);
            foreach (GoodsData data in goods) {
                WarehouseGoodsUI goodsUI = Instantiate(this.GoodsPrefab, GoodsContainer);
                goodsUI.SetContent(data, canUse && data.IsConsumeGoods);
                goodsUI.OnClicked += goodsName => OnClickGoods?.Invoke(goodsName);
            }
        }
        this.gameObject.SetActive(true);
    }

    public void Hide() {
        this.CurrentShowGoodsType = GoodsType.None;
        this.gameObject.SetActive(false);
    }

}




