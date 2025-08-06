using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{

    [SerializeField] private PassiveEntryTooltip TooltipPrefab;
    
    public PassiveEntry passiveEntryData;
    private TextMeshProUGUI PassiveEntryNameText;

    private PassiveEntryTooltip CurrentTooltip;
    private RectTransform PassiveEntryRect;

    private void Awake(){
        PassiveEntryNameText = GetComponentInChildren<TextMeshProUGUI>();
        PassiveEntryRect = this.GetComponent<RectTransform>();
    }

    private void Start(){
        if (passiveEntryData != null && PassiveEntryNameText != null){
            PassiveEntryNameText.text = passiveEntryData.Data.Name;
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        if (passiveEntryData != null){
            this.CurrentTooltip = Instantiate(TooltipPrefab, BattleUIManager.Instance.UICanvas.transform);
            this.CurrentTooltip.ShowTooltip(passiveEntryData.Data.Description,
                PassiveEntryRect.position + Vector3.up * (PassiveEntryRect.sizeDelta.y / 2.2f));
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        this.CurrentTooltip.HideTooltip();
        Destroy(this.CurrentTooltip.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData){
        int recall = BattleManager.Instance.AddPassiveEntry(passiveEntryData);
        if (recall >= 0){
            Destroy(gameObject);
        }
    }
}