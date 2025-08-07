
using UnityEngine;
using UnityEngine.UI;

public class HeroPanelUI : MonoBehaviour{

    [SerializeField] private Image HeroPortraitImage;

    public void SetPortrait(Sprite heroPortrait){
        HeroPortraitImage.sprite = heroPortrait;
    }

    public void HeroDead(){
        Material newMat = new Material(HeroPortraitImage.material);
        newMat.SetFloat(MaterialProperty.Desaturation, 0);
        HeroPortraitImage.material = newMat;
        HeroPortraitImage.color = Color.gray;
    }

}

