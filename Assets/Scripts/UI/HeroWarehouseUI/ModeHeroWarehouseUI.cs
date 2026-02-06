using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeHeroWarehouseUI : MonoBehaviour {
    
    public enum WarehouseMode {
        CharacterDisplay, // 角色展示
        CharacterBond     // 角色羁绊
    }

    [Header("当前模式")]
    [SerializeField] private WarehouseMode currentMode = WarehouseMode.CharacterDisplay;
    [SerializeField] private Image ModeImage;
    [SerializeField] private Sprite CharacterDisplayModeSprite;
    [SerializeField] private Sprite CharacterBondModeSprite;

    [Header("Panel")]
    [SerializeField] private HeroDisplayPanelUI HeroDisplayPanel;
    [SerializeField] private HeroBondPanelUI HeroBondPanel;
    [SerializeField] private HeroWarehouseListUI HeroListPanel;

    public void Show() {
        SwitchModeToCharacterDisplay();
        this.HeroListPanel.AllCategory(true);
    }

    private void SwitchModeToCharacterDisplay() {
        if (currentMode == WarehouseMode.CharacterDisplay) return;
        currentMode = WarehouseMode.CharacterDisplay;

        this.HeroDisplayPanel.Show();
        this.HeroBondPanel.Hide();
        this.ModeImage.sprite = CharacterDisplayModeSprite;
    }

    public void ShowHeroDisplay(string heroName) {
        this.HeroDisplayPanel.Show(heroName);
    }

    private void SwitchModeToCharacterBond() {
        if(currentMode == WarehouseMode.CharacterBond) return;
        currentMode = WarehouseMode.CharacterBond;

        this.HeroDisplayPanel.Hide();
        this.HeroBondPanel.Show();
        this.ModeImage.sprite = CharacterBondModeSprite;
    }
}
