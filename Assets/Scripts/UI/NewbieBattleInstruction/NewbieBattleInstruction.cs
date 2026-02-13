

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewbieBattleInstruction : MonoBehaviour, IPointerClickHandler {
    
    private CanvasGroup CanvasGroup;
    private int CurrentInstructionIndex = -1;

    private void Awake() {
        this.CanvasGroup = this.GetComponent<CanvasGroup>();
        this.Transition(false);
    }

    private void Transition(bool show) {
        this.CanvasGroup.interactable = show;
        this.CanvasGroup.blocksRaycasts = show;
    }

    private bool IsSatisfyCondition() {
        if (this.CurrentInstructionIndex == 0) return true;
        if (this.CurrentInstructionIndex == 1) return BattleUIManager.Instance.heroDetailUI.gameObject.activeSelf;
        if (this.CurrentInstructionIndex <= 4) return true;
        if (this.CurrentInstructionIndex == 5) return HeroWarehouseManager.Instance.OwnedHeroesCount >= 2;
        if (this.CurrentInstructionIndex == 6)
            return BattleUIManager.Instance.heroDetailUI.gameObject.activeSelf &&
                   PassiveEntryWarehouseManager.Instance.HasPassiveEntry;
        return false;
    }

    private void Update() {
        this.CurrentInstructionIndex = SaveDataManager.Instance.PlayerData.BattleInstructionIndex;
        if (CurrentInstructionIndex >= this.transform.childCount) {
            this.enabled = false;
            return;
        }

        if (IsSatisfyCondition()) {
            this.Transition(true);
            this.transform.GetChild(CurrentInstructionIndex).gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        this.Transition(false);
        this.transform.GetChild(CurrentInstructionIndex).gameObject.SetActive(false);
        SaveDataManager.Instance.PlayerData.BattleInstructionIndex += 1;
    }
}


