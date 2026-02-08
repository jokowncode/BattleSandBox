

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroTacticUI : MonoBehaviour {

    [SerializeField] private Button UseTacticButton;
    [SerializeField] private TextMeshProUGUI TacticDesc;
    [SerializeField] private TextMeshProUGUI TacticName;
    [SerializeField] private TextMeshProUGUI TacticCount;

    public void SetContent(BattleTacticType type, bool canUseTactic) {
        if (type == BattleTacticType.None) return;
        string tacticName = type.ToString();
        int count = GoodsWarehouseManager.Instance.GetGoodsCount(tacticName);
        this.TacticDesc.text = BattleTacticFactory.GetBattleTacticDescription(type);
        this.TacticCount.text = count.ToString("D3");
        this.TacticName.text = tacticName;

        this.UseTacticButton.enabled = canUseTactic && count != 0;
        if (!canUseTactic) return;
        this.UseTacticButton.onClick.AddListener(() => {
            GoodsWarehouseManager.Instance.UseConsumedGoods(tacticName, type);
        });
    }
}

