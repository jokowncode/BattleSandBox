using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SkillData skillData;
    public TextMeshProUGUI skillNameText;

    private void Start()
    {
        if (skillData != null && skillNameText != null)
        {
            skillNameText.text = skillData.Name;
            this.GetComponent<Image>().color = skillData.UIColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillData != null)
        {
            TooltipManager.Instance.ShowTooltip(skillData.Description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on skill: {skillData.Name}");
    
        int recall = BattleManager.Instance.AddSkill(skillData);

        if (recall >= 0)
        {
            Debug.Log("技能添加成功，销毁当前物体");
            Destroy(gameObject); // 销毁当前点击的UI或物体
            TooltipManager.Instance.HideTooltip();
        }
        else
        {
            Debug.LogWarning("技能槽已满，未添加技能");
        }
    }
}