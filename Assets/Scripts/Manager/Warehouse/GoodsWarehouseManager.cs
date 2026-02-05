
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class GoodsWarehouseManager : MonoBehaviour {

    public static GoodsWarehouseManager Instance;

    [field: SerializeField] public List<StoreGoodsData> AllGoodsData { get; private set; }

    private Dictionary<string, StoreGoodsData> AllStoreGoodsMap;

    private SerializableDictionary<string, int> OwnedConsumedGoods;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        this.AllStoreGoodsMap = new Dictionary<string, StoreGoodsData>();
        foreach (StoreGoodsData storeGoodsData in AllGoodsData) {
            this.AllStoreGoodsMap.Add(storeGoodsData.GoodsName, storeGoodsData);
        }
    }
    
    private void Start() {
        SaveDataManager.Instance.OnLoadData += () => {
            this.OwnedConsumedGoods = SaveDataManager.Instance.PlayerData.OwnedConsumedGoods;
        };
    }

    public StoreGoodsData GetGoodsData(string goodsName) {
        return this.AllStoreGoodsMap.GetValueOrDefault(goodsName);
    }

    public int GetGoodsCount(string goodsName) {
        return this.OwnedConsumedGoods.ContainsKey(goodsName) ? this.OwnedConsumedGoods[goodsName] : 0;
    }

    private void AddConsumeGoods(string goodsName) {
        if (!this.AllStoreGoodsMap.ContainsKey(goodsName)) return;
        if (!this.OwnedConsumedGoods.ContainsKey(goodsName)) {
            this.OwnedConsumedGoods.Add(goodsName, 1);
        } else {
            this.OwnedConsumedGoods[goodsName] += 1;
        }
    }

    public void UseConsumedGoods(string goodsName, params Object[] args) {
        if (!this.OwnedConsumedGoods.ContainsKey(goodsName)) return;
        if (!this.AllStoreGoodsMap.ContainsKey(goodsName)) return;

        this.OwnedConsumedGoods[goodsName] -= 1;
        if (this.OwnedConsumedGoods[goodsName] <= 0) {
            this.OwnedConsumedGoods.Remove(goodsName);
        }

        StoreGoodsData goodsData = this.AllStoreGoodsMap[goodsName];
        switch (goodsData.Type) {
            case GoodsType.EXP:
                if (args.Length < 2) return;
                EntanglementManager.Instance.AddEntanglementValue(args[0].ToString(), args[1].ToString(), goodsData.Value);
                break;
            case GoodsType.BloodBottle:
                if (args.Length < 1) return;
                SaveDataManager.Instance.RecoverHeroHealth(args[0].ToString(), goodsData.Value, false);
                break;
            case GoodsType.Tactic:
                if (args.Length < 1) return;
                UISelectionManager.Instance.UseTactic((BattleTacticType)args[0]);
                break;
        }
    }

    public bool AddGoods(StoreGoodsData data) {
        switch (data.Type) {
            case GoodsType.Hero:
                return HeroWarehouseManager.Instance.AddHero(data.GoodsName);
            case GoodsType.PassiveEntry:
                PassiveEntryWarehouseManager.Instance.AddPassiveEntry(data.GoodsName, 1);
                break;
            case GoodsType.Tactic:
            case GoodsType.EXP:
            case GoodsType.BloodBottle:
                this.AddConsumeGoods(data.GoodsName);
                break;
        }
        return true;
    }
}


