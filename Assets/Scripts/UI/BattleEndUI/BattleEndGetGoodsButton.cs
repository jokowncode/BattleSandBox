
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleEndGetGoodsButton : DetailButton, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private TextMeshProUGUI GoodsTypeText;

    public override void SetData(string desc, string showName, int count, bool canUse, GoodsType type, string actualName = null) {
        base.SetData(desc, showName, count, canUse, type, actualName);
        if (this.GoodsTypeText) this.GoodsTypeText.text = type.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(this.GoodsTypeText) this.GoodsTypeText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(this.GoodsTypeText) this.GoodsTypeText.gameObject.SetActive(false);
    }
}


