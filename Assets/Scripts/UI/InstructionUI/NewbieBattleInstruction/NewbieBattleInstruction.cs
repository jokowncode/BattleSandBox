

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewbieBattleInstruction : MonoBehaviour {

    [SerializeField] private InstructionMask Mask;
    
    private CanvasGroup CanvasGroup;
    private int CurrentInstructionIndex = -1;

    private void Awake() {
        this.CanvasGroup = this.GetComponent<CanvasGroup>();
        this.Transition(false);
        this.Mask.OnInstructionMaskClicked += this.OnClick;
    }

    private void Transition(bool show) {
        this.CanvasGroup.interactable = show;
        this.CanvasGroup.blocksRaycasts = show;
        if (this.Mask) {
            if (show) {
                RectTransform targetRect = (RectTransform)this.transform.GetChild(CurrentInstructionIndex);
                this.Mask.Show(targetRect, targetRect.sizeDelta, true);
            }
        }

        UISelectionManager.Instance.StopTime(show);
    }

    private bool IsSatisfyCondition() {
        if (this.CurrentInstructionIndex == 0) return true;
        if (this.CurrentInstructionIndex == 1) return BattleUIManager.Instance.heroDetailUI.gameObject.activeSelf;
        if (this.CurrentInstructionIndex <= 5) return true;
        if (this.CurrentInstructionIndex == 6) return BattleManager.Instance.IsBattleStart;
        if (this.CurrentInstructionIndex == 7) return HeroWarehouseManager.Instance.OwnedHeroesCount >= 2;
        if (this.CurrentInstructionIndex == 8) {
            return BattleManager.Instance.IsVictory;
        }
        if (this.CurrentInstructionIndex == 9) return true;
        if (this.CurrentInstructionIndex == 10)
            return BattleUIManager.Instance.heroDetailUI.gameObject.activeSelf &&
                   PassiveEntryWarehouseManager.Instance.HasPassiveEntry;
        
        if (this.CurrentInstructionIndex == 11) {
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

        if (this.CurrentInstructionIndex == 12) {
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

        GameObject instructionGo = this.transform.GetChild(CurrentInstructionIndex).gameObject;
        if (!instructionGo.activeSelf && IsSatisfyCondition()) {
            this.Transition(true);
            instructionGo.SetActive(true);
        }
    }

    private void OnClick() {
        this.Transition(false);
        this.transform.GetChild(CurrentInstructionIndex).gameObject.SetActive(false);
        SaveDataManager.Instance.PlayerData.BattleInstructionIndex += 1;
    }
}


