
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

        int bloodBottleCount = Random.value > 0.5f ? 2 : 1;
        int expCount = Random.value > 0.5f ? 2 : 1;
        this.InstantiateConsumeGoodsByGoodsType(GoodsType.血瓶, bloodBottleCount);
        this.InstantiateConsumeGoodsByGoodsType(GoodsType.经验, expCount);
        
        List<VictoryFixedGoodsData> datas = BattleManager.Instance.Data.FixedGetGoods;
        foreach (VictoryFixedGoodsData data in datas) {
            this.InstantiateGoodsItem(data.Data, data.Count);
        }
        this.RandomPassiveEntry();
    }

    private void RandomPassiveEntry() {
        float star1 = Random.value;
        int star1_count = star1 > 0.85f ? 2 : (star1 > 0.7f ? 1 : 0);
        int star2_count = Random.value > 0.95f ? 1 : 0;
        int star3_count = Random.value > 0.97f ? 1 : 0;
        
        InstantiateRandomPassiveEntry(1, star1_count);
        InstantiateRandomPassiveEntry(2, star2_count);
        InstantiateRandomPassiveEntry(3, star3_count);
    }

    private void InstantiateRandomPassiveEntry(int star, int count) {
        if (count == 0) return;
        PassiveEntryData result = PassiveEntryWarehouseManager.Instance.GetRandomPassiveEntryByStar(star);
        if (!result) return;
        PassiveEntryWarehouseManager.Instance.AddPassiveEntry(result.Name, count);
        DetailButton button = Instantiate(this.GetGoodsButtonPrefab, Container);
        button.SetData("", result.Name,  count, false, (GoodsType)(int)result.Rare, result.Name);
    }

    private void InstantiateConsumeGoodsByGoodsType(GoodsType type, int count) {
        StoreGoodsData data = GoodsWarehouseManager.Instance.GetFirstGoodsByType(type);
        if (!data) return;
        this.InstantiateGoodsItem(data, count);
    }

}




