

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroTacticUI : MonoBehaviour {

    [SerializeField] private Button UseTacticButton;
    [SerializeField] private TextMeshProUGUI TacticDesc;

    public void SetContent(string desc, BattleTacticType type, bool canUseTactic) {
        int count = GoodsWarehouseManager.Instance.GetGoodsCount(desc);
        this.TacticDesc.text = $"{desc} x{count}";

        this.UseTacticButton.enabled = canUseTactic && count != 0;
        if (!canUseTactic) return;
        this.UseTacticButton.onClick.AddListener(() => {
            GoodsWarehouseManager.Instance.UseConsumedGoods(desc, type);
        });
    }
}

