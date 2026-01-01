
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BigMapUIManager : MonoBehaviour{

    [SerializeField] private CanvasGroup HUDCanvasGroup;
    [SerializeField] private BattleStartUI BattleStartBannar;

    [SerializeField] private TextMeshProUGUI MoneyText;
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
        SetMoneyText(GameManager.Instance.Money);
    }

    public void SetMoneyText(float money) {
        this.MoneyText.text = money.ToString();
    }

    public void ShowBattleStartUI(Sprite background, Sprite battleImage, string battleText){
        this.BattleStartBannar.ShowBattleStartUI(background, battleImage, battleText);
    }
}

