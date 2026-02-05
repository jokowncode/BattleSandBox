

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleTacticButton : MonoBehaviour {

    [SerializeField] private Button UseTacticButton;
    [SerializeField] private TextMeshProUGUI TacticDesc;

    public void SetContent(string desc, BattleTacticType type) {
        int count = GoodsWarehouseManager.Instance.GetGoodsCount(desc);
        this.TacticDesc.text = $"{desc} x{count}";
        
        this.UseTacticButton.enabled = count != 0;
        this.UseTacticButton.onClick.AddListener(() => {
            GoodsWarehouseManager.Instance.UseConsumedGoods(desc, type);
        });
    }
}


