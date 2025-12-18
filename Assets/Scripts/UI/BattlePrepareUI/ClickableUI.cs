using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{

    [SerializeField] private PassiveEntryTooltip TooltipPrefab;
    [SerializeField] private TextMeshProUGUI PassiveEntryNameText;
    
    [HideInInspector] public PassiveEntry passiveEntryData;
    [HideInInspector] public int passiveEntryCount;

    private PassiveEntryTooltip CurrentTooltip;
    private RectTransform PassiveEntryRect;

    private void Awake(){
        PassiveEntryRect = this.GetComponent<RectTransform>();
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

    public void OnPointerClick(PointerEventData eventData) {
        Click();
    }

    private void Click() {
        int recall = BattleManager.Instance.AddPassiveEntry(passiveEntryData);
        if (recall >= 0) {
            this.UpdatePassiveEntryCount(this.passiveEntryCount - 1);
            if (passiveEntryCount > 0) return;
            if(this.CurrentTooltip) Destroy(this.CurrentTooltip.gameObject);
            Destroy(gameObject);
        }
    }

    public void UpdatePassiveEntryCount(int count) {
        this.passiveEntryCount = count;
        this.PassiveEntryNameText.text = this.passiveEntryData.Data.Name + (count > 1 ? "*"+count : "");
    }
}