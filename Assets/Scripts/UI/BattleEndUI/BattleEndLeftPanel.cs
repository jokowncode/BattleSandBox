
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleEndLeftPanel : MonoBehaviour {

    [SerializeField] private GameObject BondUpGO;

    [SerializeField] private Transform Container;
    [SerializeField] private DetailButton GetGoodsButtonPrefab;
    [SerializeField] private TextMeshProUGUI MoneyText;

    private void InstantiateGoodsItem(StoreGoodsData data, int count) {
        GoodsWarehouseManager.Instance.AddGoods(data, count);
        DetailButton button = Instantiate(this.GetGoodsButtonPrefab, Container);
        button.SetData("", data.GoodsShowName,  count, false, data.Type, data.GoodsName);
    }
    
    public void Show(bool victory) {
        this.BondUpGO.SetActive(victory);
        if (!victory) {
            foreach (Transform child in this.Container) {
                Destroy(child.gameObject);
            }
            return;
        }

        int getMoney = BattleManager.Instance.Data.Money;
        GameManager.Instance.SetMoney(GameManager.Instance.Money + getMoney);
        this.MoneyText.text = $"货币：{getMoney}";

        List<VictoryFixedGoodsData> datas = BattleManager.Instance.Data.FixedGetGoods;
        foreach (VictoryFixedGoodsData data in datas) {
            this.InstantiateGoodsItem(data.Data, data.Count);
        }

        this.InstantiateByGoodsType(GoodsType.血瓶, BattleManager.Instance.Data.RandomBloodBottleAmount);
        this.InstantiateByGoodsType(GoodsType.经验, BattleManager.Instance.Data.RandomExpAmount);
        this.RandomPassiveEntry(BattleManager.Instance.Data.RandomPassiveEntryAmount, 
            BattleManager.Instance.Data.RandomPassiveEntryMaxStar);
    }

    private void RandomPassiveEntry(int count, int maxStar) {
        maxStar = Mathf.Min(maxStar, 2);
        Dictionary<string, int> result = PassiveEntryWarehouseManager.Instance.GetRandomPassiveEntryDataByStar(count, maxStar);
        foreach (var pair in result) {
            PassiveEntryWarehouseManager.Instance.AddPassiveEntry(pair.Key, pair.Value);
            DetailButton button = Instantiate(this.GetGoodsButtonPrefab, Container);
            button.SetData("", pair.Key,  pair.Value, false, GoodsType.普通词条, pair.Key);
        }
    }

    private void InstantiateByGoodsType(GoodsType type, int count) {
        Dictionary<StoreGoodsData, int> result = GoodsWarehouseManager.Instance.GetRandomGoods(type, count);
        foreach (var pair in result) {
            this.InstantiateGoodsItem(pair.Key, pair.Value);
        }
    }

}




