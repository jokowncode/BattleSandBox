
using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanelUI : MonoBehaviour{

    [SerializeField] private Image HeroPortraitImage;

    [SerializeField] private GameObject EnergyBar;
    [SerializeField] private GameObject BloodBar;
    [SerializeField] private Image EnergyProgress;
    [SerializeField] private Image BloodProgress;

    public bool EnergyIsFull => Mathf.Approximately(this.EnergyProgress.fillAmount, 1.0f);

    public RectTransform Rect;

    private void Awake() {
        this.Rect = this.GetComponent<RectTransform>();
    }

    public void SetPortrait(Sprite heroPortrait, bool hasEnergy, bool hasBlood) {
        HeroPortraitImage.sprite = heroPortrait;
        this.EnergyBar.SetActive(hasEnergy);
        this.BloodBar.SetActive(hasBlood);
    }

    public void HeroDead(){
        Material newMat = new Material(HeroPortraitImage.material);
        newMat.SetFloat(MaterialProperty.Desaturation, 0);
        HeroPortraitImage.material = newMat;
        HeroPortraitImage.color = Color.gray;
    }

    public void SetHeroEnergy(float value) {
        this.EnergyProgress.fillAmount = value;
    }

    public void SetHeroBlood(float value) {
        this.BloodProgress.fillAmount = value;
    }
}

