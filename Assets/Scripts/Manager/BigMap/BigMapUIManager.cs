
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BigMapUIManager : MonoBehaviour{

    [SerializeField] private CanvasGroup HUDCanvasGroup;
    [SerializeField] private BattleStartUI BattleStartBannar;

    [SerializeField] private InstructionMask Mask;
    [SerializeField] private GameObject HUDButtonInstruction;

    [field: SerializeField] public TaskList TaskList { get; private set; }

    public static BigMapUIManager Instance;

    private Store CurrentShowStore;
    public bool IsOpenStore => this.CurrentShowStore;

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        DialogEventManager.Instance.AddEvent("ShowHUDButtonInstruction", () => {
            RectTransform targetRect = (RectTransform)this.HUDButtonInstruction.transform;
            this.Mask.Show(targetRect, targetRect.sizeDelta);
            this.HUDButtonInstruction.SetActive(true);
            GoodsWarehouseManager.Instance.SetHUDButtonsInteractable(false);
        });
        
        DialogEventManager.Instance.AddEvent("HideHUDButtonInstruction", () => {
            this.Mask.Hide();
            this.HUDButtonInstruction.SetActive(false);
            GoodsWarehouseManager.Instance.SetHUDButtonsInteractable(true);
        });
    }

    public void ShowBattleStartUI(SceneType battleScene, Sprite background, Sprite battleImage, string battleText){
        this.BattleStartBannar.ShowBattleStartUI(battleScene, background, battleImage, battleText);
    }
}

