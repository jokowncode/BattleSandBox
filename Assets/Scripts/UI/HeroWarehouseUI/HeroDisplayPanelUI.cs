
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HeroDisplayPanelUI : HeroWarehousePanelWithGoods {

    private enum ChildPanelType {
        None,
        Battle,
        Voice,
        Story
    }

    [Header("角色显示")] 
    [SerializeField] private float TimeMargin = 0.2f;
    [SerializeField] private Image MiddleHeroImage;
    [SerializeField] private TextMeshProUGUI MiddleHeroNameText;
    [SerializeField] private TextMeshProUGUI HeroEnglishNameText;
    [SerializeField] private TextMeshProUGUI HeroChineseNameText;
    [SerializeField] private HeroAudioDialog AudioDialog;
    [SerializeField] private Image BloodProgressBar;
    
    [Header("角色展示子面板")]
    [SerializeField] private HeroDisplayBattlePanel BattlePanel;
    [SerializeField] private HeroDisplayVoicePanel VoicePanel;
    [SerializeField] private HeroDisplayStoryPanel StoryPanel;
    
    private Hero CurrentDisplayHero;
    private WaitForSeconds MiddleHeroAnimsTimer;
    
    private ChildPanelType CurrentPanelType = ChildPanelType.None;

    private void Awake() {
        this.MiddleHeroAnimsTimer = new WaitForSeconds(this.TimeMargin);
    }

    public void Show(string heroName = null) {
        if (heroName != null) {
            this.CurrentDisplayHero = HeroWarehouseManager.Instance.GetHeroByRef(heroName);
        }
        
        if (!this.CurrentDisplayHero) return;
        this.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(MiddleHeroAnimsCoroutine(this.CurrentDisplayHero.WarehouseData.MiddleSpriteAnims));
        this.MiddleHeroNameText.text = this.CurrentDisplayHero.WarehouseData.HeroEnglishName;
        this.HeroEnglishNameText.text = this.CurrentDisplayHero.WarehouseData.HeroEnglishName;
        this.HeroChineseNameText.text = this.CurrentDisplayHero.WarehouseData.HeroChineseName;
        
        UpdateBloodBar();
        OpenBattlePanel(true);
        // ShowAudioDialog(this.CurrentDisplayHero.WarehouseData.GetHeroAudio(HeroAudioType.展示));
    }

    public void AddBloodBottle() {
        if (!this.CurrentDisplayHero) return;
        this.TransitionGoodsWarehouse();
    }

    public void UseBloodBottle(string goodsName) {
        if (!this.CurrentDisplayHero) return;
        GoodsWarehouseManager.Instance.UseConsumedGoods(goodsName, this.CurrentDisplayHero.Name);
        UpdateBloodBar();
    }

    private void UpdateBloodBar() {
        float health = SaveDataManager.Instance.GetHeroHealth(this.CurrentDisplayHero.Name);
        health = Mathf.Min(health, this.CurrentDisplayHero.InitialData.Health);
        this.BloodProgressBar.fillAmount = health < 0.0f ? 1.0f : health / this.CurrentDisplayHero.InitialData.Health;
    }

    public void ShowAudioDialog(HeroAudioData data) {
        this.AudioDialog.Show(data);
    }

    private IEnumerator MiddleHeroAnimsCoroutine(Sprite[] anims) {
        if (anims.Length == 0) yield break;
        int index = 0;
        while (true) {
            this.MiddleHeroImage.sprite = anims[index];
            index = (index + 1) % anims.Length;
            yield return this.MiddleHeroAnimsTimer;
        }
    }

    public void Hide() {
        this.CurrentPanelType = ChildPanelType.None;
        StopAllCoroutines();
        this.HideAudioDialog();
        this.gameObject.SetActive(false);
    }

    private void HideAudioDialog() {
        this.AudioDialog.Hide();
    }

    public override void GoBackToNormal() {
        base.GoBackToNormal();
        this.HideAudioDialog();
    }

    public void OpenBattlePanel(bool isForce) {
        OpenChildPanel(ChildPanelType.Battle, isForce);
    }

    public void OpenVoicePanel() {
        OpenChildPanel(ChildPanelType.Voice);
    }

    public void OpenStoryPanel() {
        OpenChildPanel(ChildPanelType.Story);
    }

    private void OpenChildPanel(ChildPanelType panelType, bool isForce = false) {
        if (this.CurrentPanelType == panelType && !isForce) return;
        this.CurrentPanelType = panelType;
        this.HideAudioDialog();
        SetAllSubPanelsActive();
        HeroDisplayChildPanel childPanel = panelType switch {
            ChildPanelType.Battle => this.BattlePanel,
            ChildPanelType.Voice => this.VoicePanel,
            ChildPanelType.Story => this.StoryPanel,
            _ => null
        };

        if (!childPanel) return;
        childPanel.Show(this.CurrentDisplayHero);
    }

    private void SetAllSubPanelsActive() {
        if (BattlePanel) BattlePanel.Hide();
        if (VoicePanel) VoicePanel.Hide();
        if (StoryPanel) StoryPanel.Hide();
    }

}


