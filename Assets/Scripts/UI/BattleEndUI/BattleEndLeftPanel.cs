
using System.Collections.Generic;
using UnityEngine;

public class BattleEndLeftPanel : MonoBehaviour {

    [SerializeField] private GameObject BondUpGO;

    [SerializeField] private Transform Container;
    [SerializeField] private DetailButton GetGoodsButtonPrefab;
    [SerializeField] private Sprite MoneyIcon;

    public void Show(bool victory) {
        this.BondUpGO.SetActive(victory);
        if (!victory) return;

        int getMoney = BattleManager.Instance.Data.Money;
        GameManager.Instance.SetMoney(GameManager.Instance.Money + getMoney);
        DetailButton money = Instantiate(this.GetGoodsButtonPrefab, this.Container);
        money.SetData("",$"货币：{getMoney}", 1, false, GoodsType.None);
        money.SetIcon(null, this.MoneyIcon);
        
        List<StoreGoodsData> goods = BattleManager.Instance.GetVictoryGetGoods();
        foreach (StoreGoodsData data in goods) {
            DetailButton button = Instantiate(this.GetGoodsButtonPrefab, Container);
            button.SetData("", data.GoodsShowName,  1, false, data.Type, data.GoodsName);
        }
    }

}




