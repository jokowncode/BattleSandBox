
using System;
using System.Collections.Generic;
using UnityEngine;

public class BigMapUIManager : MonoBehaviour{

    [SerializeField] private CanvasGroup HUDCanvasGroup;
    [SerializeField] private StoreUI Store;
    [SerializeField] private BattleStartUI BattleStartBannar;
    
    public static BigMapUIManager Instance;

    private Store CurrentShowStore;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowStore(Store store) {
        Store.ShowStoreUI(store.Goods);
        this.CurrentShowStore = store;
    }

    public void HideStore() {
        Store.HideStoreUI();
        this.CurrentShowStore = null;
    }

    public void RemoveStoreGoods(StoreGoodsData data) {
        if (!this.CurrentShowStore) return;
        this.CurrentShowStore.RemoveGoods(data);
        this.Store.UpdateStoreUI(this.CurrentShowStore.Goods);
    }

    public void ShowBattleStartUI(Sprite background, Sprite battleImage, string battleText){
        this.BattleStartBannar.ShowBattleStartUI(background, battleImage, battleText);
    }
}

