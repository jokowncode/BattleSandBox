
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;
using Random = UnityEngine.Random;

public struct GoodsData {
    public string Name;
    public string ShowName;
    public int GoodsCount;
    public string Desc;
    public GoodsType Type;
}

public class GoodsWarehouseManager : MonoBehaviour {

    public static GoodsWarehouseManager Instance;

    [field: SerializeField] public List<StoreGoodsData> AllGoodsData { get; private set; }
    [SerializeField] private AudioClip UseConsumeGoodsErrorSfx;
    [SerializeField] private List<GoodsImageData> ImageDatas;

    [Header("UI")] 
    [SerializeField] private GoodsWarehousePanel GoodsPanel;
    
    private Dictionary<string, StoreGoodsData> AllStoreGoodsMap;

    private Dictionary<string, int> InBattleModifyGoods = new();
    private SerializableDictionary<string, int> OwnedConsumedGoods;
    private bool IsInBattle = false;

    private Dictionary<GoodsType, GoodsImageData> ImageDataMap = new();
    
    private CanvasGroup GoodsPanelCanvasGroup;
    public bool IsOpen => this.GoodsPanelCanvasGroup.alpha >= 0.9f;

    private Player CurrentPlayer;
    
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
            this.CurrentPlayer = FindObjectOfType<Player>();
        };
        this.GoodsPanelCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    public void TransitionGoodsPanel(bool show) {
        if (show && this.GoodsPanelCanvasGroup.alpha >= 0.9f) return;
        this.GoodsPanelCanvasGroup.alpha = show ? 1.0f : 0.0f;
        this.GoodsPanelCanvasGroup.interactable = show;
        this.GoodsPanelCanvasGroup.blocksRaycasts = show;

        if (show) {
            if(this.CurrentPlayer) this.CurrentPlayer.TransMove(false);
            this.GoodsPanel.Show();
        } else {
            if(this.CurrentPlayer) this.CurrentPlayer.TransMove(true);
            this.GoodsPanel.Hide();
        }
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

    private void AddConsumeGoods(string goodsName, int count) {
        if (!this.AllStoreGoodsMap.ContainsKey(goodsName)) return;
        if (!this.OwnedConsumedGoods.ContainsKey(goodsName)) {
            this.OwnedConsumedGoods.Add(goodsName, count);
        } else {
            this.OwnedConsumedGoods[goodsName] += count;
        }
    }

    public bool UseConsumedGoods(string goodsName, params Object[] args) {
        if (!this.OwnedConsumedGoods.ContainsKey(goodsName)) return false;
        if (!this.AllStoreGoodsMap.ContainsKey(goodsName)) return false;

        bool result = true;
        StoreGoodsData goodsData = this.AllStoreGoodsMap[goodsName];
        switch (goodsData.Type) {
            case GoodsType.经验:
                if (args.Length < 2) return false;
                result = EntanglementManager.Instance.AddEntanglementValue(args[0].ToString(), args[1].ToString(), goodsData.Value);
                break;
            case GoodsType.血瓶:
                if (args.Length < 1) return false;
                result = SaveDataManager.Instance.RecoverHeroHealth(args[0].ToString(), goodsData.Value, false);
                break;
            case GoodsType.战术:
                if (!Enum.TryParse(goodsName, true, out BattleTacticType type)) return false;
                result = UISelectionManager.Instance.UseTactic(type);
                break;
            default: return false;
        }

        if (!result) {
            if (this.UseConsumeGoodsErrorSfx) {
                AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.UseConsumeGoodsErrorSfx);
            }
            return false;
        }

        this.OwnedConsumedGoods[goodsName] -= 1;
        if (this.IsInBattle) {
            this.InBattleModifyGoods.TryAdd(goodsName, 0);
            this.InBattleModifyGoods[goodsName] += 1;
        }

        if (this.OwnedConsumedGoods[goodsName] <= 0) {
            this.OwnedConsumedGoods.Remove(goodsName);
        }
        return true;
    }

    public bool AddGoods(StoreGoodsData data, int count = 1) {
        switch (data.Type) {
            case GoodsType.角色:
                return HeroWarehouseManager.Instance.AddHero(data.GoodsName);
            case GoodsType.普通词条:
            case GoodsType.特殊词条:
                PassiveEntryWarehouseManager.Instance.AddPassiveEntry(data.GoodsName, count);
                break;
            case GoodsType.战术:
            case GoodsType.经验:
            case GoodsType.血瓶:
                this.AddConsumeGoods(data.GoodsName, count);
                break;
        }
        return true;
    }

    private bool IsConsumeGoods(GoodsType type) {
        return type != GoodsType.角色 && type != GoodsType.普通词条 && type != GoodsType.特殊词条;
    }

    public GoodsImageData GetImageData(GoodsType type) {
        return this.ImageDataMap.GetValueOrDefault(type);
    }

    public Dictionary<StoreGoodsData, int> GetRandomGoods(GoodsType type, int maxCount) {
        Dictionary<StoreGoodsData, int> result = new Dictionary<StoreGoodsData, int>();
        if (!IsConsumeGoods(type)) return result;
        if (maxCount == 0) return result;

        int rest = maxCount;
        foreach (StoreGoodsData goods in this.AllGoodsData) {
            if (goods.Type != type) continue;
            int count = Mathf.Min(1, rest);
            rest -= count;
            result.Add(goods, count);
            if (rest == 0) break;
        }
        return result;
    }

    public List<GoodsData> GetGoodsByType(GoodsType type) {
        List<GoodsData> result = new();
        if (IsConsumeGoods(type)) {
            foreach (KeyValuePair<string, int> goodsPair in this.OwnedConsumedGoods) {
                StoreGoodsData data = GetGoodsData(goodsPair.Key);
                if (!data || data.Type != type) continue;
                string desc = "";
                if (data.Type == GoodsType.战术 && Enum.TryParse(data.GoodsName, true, out BattleTacticType bType)) {
                    desc = BattleTacticFactory.GetBattleTacticDescription(bType);
                }
                result.Add(new GoodsData() {
                    Name = data.GoodsName,
                    ShowName = data.GoodsShowName,
                    GoodsCount = goodsPair.Value,
                    Type = type,
                    Desc = desc
                });
            }
        }
        
        if (type is GoodsType.普通词条 or GoodsType.特殊词条) {
            Dictionary<PassiveEntry, int> entries = PassiveEntryWarehouseManager.Instance.GetPassiveEntryFilterBySort(0x7FFFFFFF);
            foreach (var pair in entries) {
                result.Add(new GoodsData() {
                    Name = pair.Key.Data.Name,
                    ShowName = pair.Key.Data.Name,
                    GoodsCount = pair.Value,
                    Type = (GoodsType)(int)pair.Key.Data.Rare,
                    Desc = pair.Key.Data.Description
                });
            }
        }
        return result;
    }
}


