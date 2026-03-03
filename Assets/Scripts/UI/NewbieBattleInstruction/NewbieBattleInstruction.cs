

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewbieBattleInstruction : MonoBehaviour, IPointerClickHandler {

    [SerializeField] private GameObject Mask;
    
    private CanvasGroup CanvasGroup;
    private int CurrentInstructionIndex = -1;

    private void Awake() {
        this.CanvasGroup = this.GetComponent<CanvasGroup>();
        this.Transition(false);
    }

    private void Transition(bool show) {
        this.CanvasGroup.interactable = show;
        this.CanvasGroup.blocksRaycasts = show;
        if(this.Mask) this.Mask.SetActive(show);
        UISelectionManager.Instance.StopTime(show);
    }

    private bool IsSatisfyCondition() {
        if (this.CurrentInstructionIndex == 0) return true;
        if (this.CurrentInstructionIndex == 1) return BattleUIManager.Instance.heroDetailUI.gameObject.activeSelf;
        if (this.CurrentInstructionIndex <= 4) return true;
        if (this.CurrentInstructionIndex == 5) return BattleManager.Instance.IsBattleStart;
        if (this.CurrentInstructionIndex == 6) return HeroWarehouseManager.Instance.OwnedHeroesCount >= 2;
        if (this.CurrentInstructionIndex == 7) {
            return BattleManager.Instance.IsVictory;
        }
        if (this.CurrentInstructionIndex == 8) return true;
        if (this.CurrentInstructionIndex == 9)
            return BattleUIManager.Instance.heroDetailUI.gameObject.activeSelf &&
                   PassiveEntryWarehouseManager.Instance.HasPassiveEntry;
        
        if (this.CurrentInstructionIndex == 10) {
            if (!BattleManager.Instance.IsBattleStart) return false;
            if (BattleManager.Instance.IsGameOver) return false;
            List<string> heroes = BattleUIManager.Instance.heroPortraitUI.HeroEnergyFullList();
            if (heroes.Count < 2) return false;
            for (int i = 0; i < heroes.Count; i++) {
                for (int j = i + 1; j < heroes.Count; j++) {
                    float value = EntanglementManager.Instance.GetHeroEntanglementValue(heroes[i], heroes[j]);
                    if (value >= EntanglementManager.Instance.MinHasTacticEntangleValue) return true;
                }
            }
        }

        if (this.CurrentInstructionIndex == 11) {
            return UISelectionManager.Instance.HasOpenTacticUI;
        }
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


