
using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanelUI : MonoBehaviour{

    [SerializeField] private Image HeroPortraitImage;

    [SerializeField] private GameObject EnergyBar;
    [SerializeField] private Image EnergyProgress;

    public void SetPortrait(Sprite heroPortrait, bool hasEnergy) {
        HeroPortraitImage.sprite = heroPortrait;
        this.EnergyBar.SetActive(hasEnergy);
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
}

