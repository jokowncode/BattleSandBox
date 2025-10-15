
using System.Collections.Generic;
using UnityEngine;

public class Store : InteractionObject {

    [field: SerializeField] public List<StoreGoodsData> Goods { get; private set; }

    protected override void Awake() {
        base.Awake();
        // TODO: Save And Load Store Goods Purchase Condition
    }

    protected override void Interaction(){
        ShowStore();
    }

    public void ShowStore() {
        BigMapUIManager.Instance.ShowStore(this);
    }

    public void RemoveGoods(StoreGoodsData data) {
        if(this.Goods.Contains(data)) this.Goods.Remove(data);
    }
}

