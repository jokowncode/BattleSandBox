
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum InstructionType {
    Clue,
    Hero
}

public class BigMapUIManager : MonoBehaviour{

    [SerializeField] private CanvasGroup HUDCanvasGroup;
    [SerializeField] private BattleStartUI BattleStartBannar;
    [SerializeField] private TextMeshProUGUI MoneyText;
    
    [SerializeField] private InstructionMask Mask;
    [SerializeField] private GameObject HUDButtonInstruction;

    // [field: SerializeField] public TaskList TaskList { get; private set; }
    [field: SerializeField] public TaskUI TaskUI { get; private set; }

    [Header("Instruction")] 
    [SerializeField] private InstructionContainer ClueInstruction;
    [SerializeField] private InstructionContainer HeroWarehouseInstruction;
    
    public static BigMapUIManager Instance;

    private Store CurrentShowStore;
    public bool IsOpenStore => this.CurrentShowStore;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        UpdateMoneyText();
    }

    private void Start() {
        DialogEventManager.Instance.AddEvent("ShowHUDButtonInstruction", () => {
            RectTransform targetRect = (RectTransform)this.HUDButtonInstruction.transform;
            this.Mask.Show(targetRect, targetRect.sizeDelta, true);
            this.Mask.OnInstructionMaskClicked += this.HideHUDButtonInstruction;
            this.HUDButtonInstruction.SetActive(true);
            SaveDataManager.Instance.PlayerInBigMap.TransMove(false);
            // GoodsWarehouseManager.Instance.SetHUDButtonsInteractable(false);
        });
    }

    private void HideHUDButtonInstruction() {
        this.HUDButtonInstruction.SetActive(false);
        SaveDataManager.Instance.PlayerInBigMap.TransMove(true);
        // GoodsWarehouseManager.Instance.SetHUDButtonsInteractable(true);
    }

    public void ShowBattleStartUI(SceneType battleScene, Sprite background, Sprite battleImage, string battleText){
        this.BattleStartBannar.ShowBattleStartUI(battleScene, background, battleImage, battleText);
    }

    public void UpdateMoneyText() {
        this.MoneyText.text = GameManager.Instance.Money.ToString();
    }

    public void ShowInstruction(InstructionType type) {
        if (type == InstructionType.Clue && !this.ClueInstruction) return;
        if (type == InstructionType.Hero && !this.HeroWarehouseInstruction) return;
        
        InstructionContainer container = type == InstructionType.Clue ? this.ClueInstruction : this.HeroWarehouseInstruction;
        container.ActivateInstruction();
        container.OnEndInstruction += () => {
            if (type == InstructionType.Clue) {
                ClueWarehouseManager.Instance.ShowClueInstruction();
            }else if (type == InstructionType.Hero) {
                HeroWarehouseManager.Instance.ShowHeroWarehouseInstruction();
            }
        };
    }
}

