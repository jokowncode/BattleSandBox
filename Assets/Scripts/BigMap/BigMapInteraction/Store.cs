
using System.Collections.Generic;
using UnityEngine;

public class Store : InteractionObject {

    [field: SerializeField] public List<StoreGoodsData> Goods { get; private set; }

    protected override void Awake() {
        base.Awake();
        // TODO: Save And Load Store Goods Purchase Condition
    }

    protected override void Interaction() {
        if (BigMapUIManager.Instance.IsOpenStore) return;
        ShowStore();
    }

    public void ShowStore() {
        StoreUI.Instance.ShowStoreUI(this);
    }

    public void RemoveGoods(StoreGoodsData data) {
        if(this.Goods.Contains(data)) this.Goods.Remove(data);
    }
}

