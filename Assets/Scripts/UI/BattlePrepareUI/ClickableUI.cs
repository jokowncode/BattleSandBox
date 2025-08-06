using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    
    public PassiveEntry passiveEntryData;
    public TextMeshProUGUI skillNameText;

    private void Start(){
        if (passiveEntryData != null && skillNameText != null){
            skillNameText.text = passiveEntryData.Data.Name;
            this.GetComponent<Image>().color = passiveEntryData.Data.UIColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        if (passiveEntryData != null){
            TooltipManager.Instance.ShowTooltip(passiveEntryData.Data.Description);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        TooltipManager.Instance.HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData){
        int recall = BattleManager.Instance.AddPassiveEntry(passiveEntryData);

        if (recall >= 0){
            Debug.Log("技能添加成功，销毁当前物体");
            Destroy(gameObject); // 销毁当前点击的UI或物体
            TooltipManager.Instance.HideTooltip();
        }else{
            Debug.LogWarning("技能槽已满，未添加技能");
        }
    }
}