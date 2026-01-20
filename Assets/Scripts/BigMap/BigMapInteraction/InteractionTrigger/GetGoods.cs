
using System.Collections.Generic;
using UnityEngine;

public class GetGoods : InteractionTrigger {
    
    [SerializeField] private List<StoreGoodsData> GetGoodsNames;
    
    protected override void TriggerAction() {
        foreach (StoreGoodsData data in GetGoodsNames) {
            GoodsWarehouseManager.Instance.AddGoods(data);
        }
    }
}




