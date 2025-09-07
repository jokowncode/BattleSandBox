
using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanelUI : MonoBehaviour{

    private SimpleAnimationUI HeroPortraitAnim;
    private Image HeroPortraitImage;

    private void Awake() {
        HeroPortraitAnim = GetComponent<SimpleAnimationUI>();
        HeroPortraitImage = GetComponent<Image>();
    }

    public void SetPortrait(Sprite[] heroPortrait){
        HeroPortraitAnim.SetAnims(heroPortrait);
    }

    public void HeroDead(){
        Material newMat = new Material(HeroPortraitImage.material);
        newMat.SetFloat(MaterialProperty.Desaturation, 0);
        HeroPortraitImage.material = newMat;
        HeroPortraitImage.color = Color.gray;
        
        HeroPortraitAnim.ResetAnim();
        HeroPortraitAnim.enabled = false;
    }

}

