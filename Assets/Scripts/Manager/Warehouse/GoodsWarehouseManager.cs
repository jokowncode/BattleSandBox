
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

public struct GoodsData {
    // public StoreGoodsData Data;
    public string Name;
    public GoodsImageData ImageData;
    public int GoodsCount;
    public bool IsConsumeGoods;
}

public class GoodsWarehouseManager : MonoBehaviour {

    public static GoodsWarehouseManager Instance;

    [field: SerializeField] public List<StoreGoodsData> AllGoodsData { get; private set; }
    [SerializeField] private AudioClip UseConsumeGoodsErrorSfx;
    [SerializeField] private List<GoodsImageData> ImageDatas; 
    
    private Dictionary<string, StoreGoodsData> AllStoreGoodsMap;

    private Dictionary<string, int> InBattleModifyGoods = new();
    private SerializableDictionary<string, int> OwnedConsumedGoods;
    private bool IsInBattle = false;

    private Dictionary<GoodsType, GoodsImageData> ImageDataMap = new();
    
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

        foreach (GoodsImageData data in this.ImageDatas) {
            this.ImageDataMap.Add(data.Type, data);
        }

        SceneManager.sceneLoaded += (arg0, mode) => {
            if (SceneTools.IsBattleScene(SceneChangeManager.Instance.CurrentScene)) {
                this.IsInBattle = true;
                this.InBattleModifyGoods.Clear();
                BattleManager.Instance.OnRewindBattle += OnRewindBattle;
            } else {
                this.IsInBattle = false;
            }
        };
    }

    private void OnRewindBattle() {
        foreach (var pair in this.InBattleModifyGoods) {
            if (this.OwnedConsumedGoods.ContainsKey(pair.Key)) {
                this.OwnedConsumedGoods[pair.Key] += pair.Value;
            } else {
                this.OwnedConsumedGoods.Add(pair.Key, pair.Value);
            }
        }
        this.InBattleModifyGoods.Clear();
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

        bool result = true;
        StoreGoodsData goodsData = this.AllStoreGoodsMap[goodsName];
        switch (goodsData.Type) {
            case GoodsType.EXP:
                if (args.Length < 2) return;
                result = EntanglementManager.Instance.AddEntanglementValue(args[0].ToString(), args[1].ToString(), goodsData.Value);
                break;
            case GoodsType.BloodBottle:
                if (args.Length < 1) return;
                result = SaveDataManager.Instance.RecoverHeroHealth(args[0].ToString(), goodsData.Value, false);
                break;
            case GoodsType.Tactic:
                if (!Enum.TryParse(goodsName, true, out BattleTacticType type)) return;
                result = UISelectionManager.Instance.UseTactic(type);
                break;
            default: return;
        }

        if (!result) {
            if (this.UseConsumeGoodsErrorSfx) {
                AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.UseConsumeGoodsErrorSfx);
            }
            return;
        }

        this.OwnedConsumedGoods[goodsName] -= 1;
        if (this.IsInBattle) {
            this.InBattleModifyGoods.TryAdd(goodsName, 0);
            this.InBattleModifyGoods[goodsName] += 1;
        }

        if (this.OwnedConsumedGoods[goodsName] <= 0) {
            this.OwnedConsumedGoods.Remove(goodsName);
        }
    }

    public bool AddGoods(StoreGoodsData data) {
        switch (data.Type) {
            case GoodsType.Hero:
                return HeroWarehouseManager.Instance.AddHero(data.GoodsName);
            case GoodsType.NormalPassiveEntry:
            case GoodsType.SpecialPassiveEntry:
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

    private bool IsConsumeGoods(GoodsType type) {
        return type != GoodsType.Hero && type != GoodsType.NormalPassiveEntry && type != GoodsType.SpecialPassiveEntry;
    }

    public GoodsImageData GetImageData(GoodsType type) {
        return this.ImageDataMap.GetValueOrDefault(type);
    }

    public List<GoodsData> GetGoodsByType(GoodsType type) {
        List<GoodsData> result = new();
        if (IsConsumeGoods(type)) {
            foreach (KeyValuePair<string, int> goodsPair in this.OwnedConsumedGoods) {
                StoreGoodsData data = GetGoodsData(goodsPair.Key);
                if (!data || data.Type != type) continue;
                result.Add(new GoodsData() {
                    Name = data.GoodsName,
                    ImageData = GetImageData(data.Type),
                    GoodsCount = goodsPair.Value,
                    IsConsumeGoods = true
                });
            }
        }
        // TODO : NOT CONSUME GOODS
        return result;
    }
}


