using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeHeroWarehouseUI : MonoBehaviour {
    
    public enum WarehouseMode {
        None,
        CharacterDisplay, // 角色展示
        CharacterBond     // 角色羁绊
    }

    [Header("Mode")]
    [SerializeField] private Image ModeImage;
    [SerializeField] private Sprite CharacterDisplayModeSprite;
    [SerializeField] private Sprite CharacterBondModeSprite;

    [Header("Panel")]
    [SerializeField] private HeroDisplayPanelUI HeroDisplayPanel;
    [SerializeField] private HeroBondPanelUI HeroBondPanel;
    [SerializeField] private HeroWarehouseListUI HeroListPanel;
    [SerializeField] private GoodsWarehousePanel GoodsWarehouseUI;

    private WarehouseMode CurrentMode = WarehouseMode.None;
    
    private void Awake() {
        this.HeroListPanel.OnHeroClicked += OnHeroClicked;
        this.GoodsWarehouseUI.OnClickGoods += OnClickGoods;
    }

    private bool OnClickGoods(string goodsName) {
        if (this.CurrentMode == WarehouseMode.CharacterBond) {
            return this.HeroBondPanel.UseBond(goodsName);
        }else if (this.CurrentMode == WarehouseMode.CharacterDisplay) {
            bool result = this.HeroDisplayPanel.UseRecoverGoods(goodsName);
            if(result) this.HeroListPanel.UpdateHeroHealth();
            return result;
        }
        return false;
    }

    public void Show() {
        this.CurrentMode = WarehouseMode.None;
        SwitchModeToCharacterDisplay();
        this.HeroListPanel.AllCategory(true);
    }

    public void Hide() {
        this.HeroDisplayPanel.GoBackToNormal();
        this.HeroBondPanel.GoBackToNormal();
    }

    private void SwitchModeToCharacterDisplay() {
        if (CurrentMode == WarehouseMode.CharacterDisplay) return;
        CurrentMode = WarehouseMode.CharacterDisplay;

        this.HeroDisplayPanel.Show();
        this.HeroBondPanel.Hide();
        this.GoodsWarehouseUI.Hide();
        this.ModeImage.sprite = CharacterDisplayModeSprite;
    }

    private void OnHeroClicked(string heroName) {
        if (CurrentMode == WarehouseMode.CharacterDisplay) {
            this.HeroDisplayPanel.Show(heroName);
        }else if (CurrentMode == WarehouseMode.CharacterBond) {
            this.HeroBondPanel.SelectHero(heroName);
        }
    }

    private void SwitchModeToCharacterBond() {
        if(CurrentMode == WarehouseMode.CharacterBond) return;
        CurrentMode = WarehouseMode.CharacterBond;

        this.HeroDisplayPanel.Hide();
        this.HeroBondPanel.Show();
        this.GoodsWarehouseUI.Hide();
        this.ModeImage.sprite = CharacterBondModeSprite;
    }

    public void ShowGoodsWarehouse(GoodsType[] type) {
        for (int i = 0; i < type.Length; i++) {
            this.GoodsWarehouseUI.Show(type[i], true, i != 0);
        }
    }

    public void HideGoodsWarehouse() {
        this.GoodsWarehouseUI.Hide();
    }
}
