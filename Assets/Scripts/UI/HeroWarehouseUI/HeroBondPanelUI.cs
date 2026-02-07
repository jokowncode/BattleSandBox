
using System;
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
    
    private Hero[] CurrentHeroes = new Hero[2];

    public void Show() {
        for (int i = 0; i < this.CurrentHeroes.Length; i++) {
            UnSelectHero(i);
        }
        this.gameObject.SetActive(true);
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

    private bool TrySelectHero(int index, string heroName) {
        if (CurrentHeroes[index]) return false;
        CurrentHeroes[index] = HeroWarehouseManager.Instance.GetHeroByRef(heroName);
        this.HeroNameTexts[index].text = CurrentHeroes[index].WarehouseData.HeroChineseName;
        this.HeroImages[index].sprite = CurrentHeroes[index].WarehouseData.MiddleSpriteAnims[0];
        this.HeroImages[index].color = Color.white;
        return true;
    }

    public void SelectHero(string heroName) {
        for (int i = 0; i < this.CurrentHeroes.Length; i++) {
            if (this.CurrentHeroes[i]) {
                if (this.CurrentHeroes[i].Name == heroName) return;
                continue;
            }
            if (!TrySelectHero(i, heroName)) continue;
            if (i == this.CurrentHeroes.Length - 1) {
                this.UpdateBondData();
            }
            return;
        }
    }

    public void UnSelectHero(int index) {
        if (!CurrentHeroes[index]) return;
        CurrentHeroes[index] = null;
        this.HeroNameTexts[index].text = "";
        this.HeroImages[index].sprite = null;
        this.HeroImages[index].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
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
        this.BondLevelText.text = data.BondLevel.ToString();

        float upgradeValue = data.NextLevelValue - data.CurrentLevelValue;
        float current = data.CurrentValue - data.CurrentLevelValue;
        
        this.LevelValueProgress.fillAmount = current / upgradeValue;
        this.TacticPanel.Show(CurrentHeroes[0].Name, CurrentHeroes[1].Name, false);
    }

    public void AddBond() {
        if (!CurrentHeroes[0] || !CurrentHeroes[1]) return;
        this.TransitionGoodsWarehouse();
    }

    public void UseBond(string goodsName) {
        if (!CurrentHeroes[0] || !CurrentHeroes[1]) return;
        GoodsWarehouseManager.Instance.UseConsumedGoods(goodsName, CurrentHeroes[0].Name, CurrentHeroes[1].Name);
        UpdateBondData();
    }
}


