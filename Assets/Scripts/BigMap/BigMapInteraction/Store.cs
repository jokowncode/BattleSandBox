
using System;
using System.Collections.Generic;
using UnityEngine;

public class Store : InteractionObject {

    [field: SerializeField] public List<string> Goods { get; private set; }

    protected override InteractionObjType GetInteractionObjType() {
        return InteractionObjType.Store;
    }

    protected override void Awake() {
        base.Awake();
        this.IsEndCanInteract = true;
    }

    protected override void LoadBigMapData() {
        if (!this.IsEnd) {
            if(PlayerPrefs.HasKey(GetName())) PlayerPrefs.DeleteKey(GetName());
        }
        
        // TODO: Random Store Refresh
        if (this.Goods == null || this.Goods.Count == 0) {
            // Random Store
            if (SceneChangeManager.Instance.IsNewDungeon) {
                if(PlayerPrefs.HasKey(GetName())) PlayerPrefs.DeleteKey(GetName());
                // TODO: Random Store Goods
                this.Goods = StoreUI.Instance.RandomGoodsSimple();
            }
        }
        
        if (PlayerPrefs.HasKey(GetName())) {
            string json = PlayerPrefs.GetString(GetName());
            this.Goods = JsonUtility.FromJson<Serialization<string>>(json).ToList();
        }
    }

    private void OnDestroy() {
        string json = JsonUtility.ToJson(new Serialization<string>(this.Goods));
        PlayerPrefs.SetString(GetName(), json);
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

