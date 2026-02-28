
using UnityEngine;

public class TacticPanelUI : MonoBehaviour {

    [SerializeField] private Transform Container;
    [SerializeField] private DetailButton BattleTacticButtonPrefab;

    public void ClearTactic() {
        foreach (Transform child in Container) {
            Destroy(child.gameObject);
        }
    }

    public void Show(string hero1, string hero2, bool canUseTactic) {
        this.gameObject.SetActive(true);
        this.ClearTactic();
        BattleTacticType maxTactic = EntanglementManager.Instance.GetEntangleHeroCanCastMaxBattleTactic(hero1, hero2);
        for (int i = 0; i <= (int) maxTactic; i++) {
            DetailButton button = Instantiate(this.BattleTacticButtonPrefab, this.Container);
            BattleTacticType type = (BattleTacticType) i;
            string tacticName = type.ToString();
            int count = GoodsWarehouseManager.Instance.GetGoodsCount(tacticName);
            string desc = BattleTacticFactory.GetBattleTacticDescription(type);
            button.SetData(desc, tacticName, count, canUseTactic, GoodsType.战术);
            button.TransitionButtonInteractable(count != 0);
            button.OnButtonClicked += (showName, _) => {
                GoodsWarehouseManager.Instance.UseConsumedGoods(showName);
            };
        }
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

}

