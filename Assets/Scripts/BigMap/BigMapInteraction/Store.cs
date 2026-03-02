
using System;
using System.Collections.Generic;
using UnityEngine;

public class Store : InteractionObject {

    [ScriptableObjectNameProp(typeof(StoreGoodsData), "GoodsName")] 
    [SerializeField] private List<string> Goods;

    public List<string> CurrentGoods => this.Goods;
    
    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.商店;
    }

    protected override void Awake() {
        base.Awake();
        this.IsEndCanInteract = true;
    }

    protected override void LoadBigMapData() {
        var storeGoods = SaveDataManager.Instance.PlayerData.StoreGoods;
        if ((this.Goods == null || this.Goods.Count == 0) && !this.IsEnd) {
            // Random Store
            if (SceneChangeManager.Instance.IsNewDungeon) {
                if(storeGoods.ContainsKey(GetName())) storeGoods.Remove(GetName());
                // TODO: Random Store Goods
                this.Goods = StoreUI.Instance.RandomGoodsSimple();
            }
        }
        
        if (!this.IsEnd) {
            if(storeGoods.ContainsKey(GetName())) storeGoods.Remove(GetName());
        }
        
        if (storeGoods.ContainsKey(GetName())) {
            string json = storeGoods[GetName()];
            this.Goods = JsonUtility.FromJson<Serialization<string>>(json).ToList();
        }
    }

    private void OnDestroy() {
        string json = JsonUtility.ToJson(new Serialization<string>(this.Goods));
        if (!SaveDataManager.Instance.PlayerData.StoreGoods.TryAdd(GetName(), json)) {
            SaveDataManager.Instance.PlayerData.StoreGoods[GetName()] = json;
        }
    }

    protected override void Interaction() {
        ShowStore();
    }

    public void ShowStore() {
        if (BigMapUIManager.Instance.IsOpenStore) return;
        StoreUI.Instance.ShowStoreUI(this);
    }

    public void RemoveGoods(string goodsName) {
        if (this.Goods.Contains(goodsName)) {
            this.Goods.Remove(goodsName);
            this.EndInteraction();
        }
    }
}

