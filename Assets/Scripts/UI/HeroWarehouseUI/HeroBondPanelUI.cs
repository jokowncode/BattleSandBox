
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroBondPanelUI : HeroWarehousePanelWithGoods {

    [Header("Hero UI")] 
    [SerializeField] private TextMeshProUGUI[] HeroNameTexts;
    [SerializeField] private Image[] HeroImages;

    [Header("Bond UI")] 
    [SerializeField] private TextMeshProUGUI BondLevelText;
    [SerializeField] private Image LevelValueProgress;
    [SerializeField] private TacticPanelUI TacticPanel;

    [Header("Bond Border")] 
    [SerializeField] private Image[] BondBorderImages;
    [SerializeField] private Sprite WarriorBorder;
    [SerializeField] private Sprite MageBorder;
    [SerializeField] private Sprite PriestBorder;
    [SerializeField] private Sprite EmptyBorder;
    
    private Hero[] CurrentHeroes = new Hero[2];

    public void Show() {
        for (int i = 0; i < this.CurrentHeroes.Length; i++) {
            UnSelectHero(i);
        }
        this.gameObject.SetActive(true);
    }

    private bool TrySelectHero(int index, string heroName) {
        if (CurrentHeroes[index]) return false;
        CurrentHeroes[index] = HeroWarehouseManager.Instance.GetHeroByRef(heroName);
        this.HeroNameTexts[index].text = CurrentHeroes[index].WarehouseData.HeroChineseName;
        this.HeroImages[index].sprite = CurrentHeroes[index].WarehouseData.HeroWarehouseSprite;
        this.HeroImages[index].color = Color.white;
        this.BondBorderImages[index].sprite = CurrentHeroes[index].Type switch {
            FighterType.Warrior => this.WarriorBorder,
            FighterType.Mage => this.MageBorder,
            FighterType.Priest => this.PriestBorder,
            _ => this.EmptyBorder
        };
        return true;
    }

    public void SelectHero(string heroName) {
        if (this.CurrentHeroes.Any(h => h && h.Name == heroName)) return;
        for (int i = 0; i < this.CurrentHeroes.Length; i++) {
            if (this.CurrentHeroes[i]) continue;
            if (TrySelectHero(i, heroName)) break;
        }

        if (CurrentHeroes.Any(hero => !hero)) return;
        UpdateBondData();
    }

    public void UnSelectHero(int index) {
        if (!CurrentHeroes[index]) return;
        CurrentHeroes[index] = null;
        this.HeroNameTexts[index].text = "";
        this.HeroImages[index].sprite = null;
        this.HeroImages[index].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        this.BondBorderImages[index].sprite = this.EmptyBorder;
        this.UpdateBondData();
        this.GoBackToNormal();
    }

    private void UpdateBondData() {
        this.TacticPanel.ClearTactic();
        if (!CurrentHeroes[0] || !CurrentHeroes[1]) {
            this.BondLevelText.text = "";
            this.LevelValueProgress.fillAmount = 0.0f;
            return;
        }

        BondData data = EntanglementManager.Instance.GetBondData(CurrentHeroes[0].Name, CurrentHeroes[1].Name);

        bool isMaxLevel = data.BondLevel >= EntanglementManager.Instance.MaxLevel;
        this.BondLevelText.text = isMaxLevel ? "MAX" : data.BondLevel.ToString();

        if (isMaxLevel) {
            this.LevelValueProgress.fillAmount = 1.0f;
        } else {
            float upgradeValue = data.NextLevelValue - data.CurrentLevelValue;
            float current = data.CurrentValue - data.CurrentLevelValue;
            this.LevelValueProgress.fillAmount = current / upgradeValue;
        }
        this.TacticPanel.Show(CurrentHeroes[0].Name, CurrentHeroes[1].Name, false);
    }

    public void AddBond() {
        if (!CurrentHeroes[0] || !CurrentHeroes[1]) return;
        this.TransitionGoodsWarehouse();
    }

    public bool UseBond(string goodsName) {
        if (!CurrentHeroes[0] || !CurrentHeroes[1]) return false;
        bool result = GoodsWarehouseManager.Instance.UseConsumedGoods(goodsName, CurrentHeroes[0].Name, CurrentHeroes[1].Name);
        if(result) UpdateBondData();
        return result;
    }
}


