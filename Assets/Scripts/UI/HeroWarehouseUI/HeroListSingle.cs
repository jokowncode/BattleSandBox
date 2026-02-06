
using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroListSingle : MonoBehaviour {

    [SerializeField] private Image BorderImage;
    [SerializeField] private Button HeroAvatarButton;
    
    private Image HeroAvatarImage;
    private ModeHeroWarehouseUI ModeHeroWarehousePanel;

    private void Awake() {
        this.HeroAvatarImage = this.HeroAvatarButton.GetComponent<Image>();
        this.ModeHeroWarehousePanel = this.GetComponentInParent<ModeHeroWarehouseUI>();
    }

    public void SetContent(Hero hero, Sprite borderSprite, Sprite heroAvatarSprite) {
        this.BorderImage.sprite = borderSprite;
        this.HeroAvatarImage.sprite = heroAvatarSprite;
        this.HeroAvatarImage.color = new Color(1.0f, 1.0f, 1.0f, heroAvatarSprite ? 1.0f : 0.0f);
        this.HeroAvatarButton.enabled = hero;
        
        this.HeroAvatarButton.onClick.RemoveAllListeners();
        this.HeroAvatarButton.onClick.AddListener(() => {
            this.ModeHeroWarehousePanel.ShowHeroDisplay(hero.Name);
        });
    }

    public void ClickButton() {
        this.HeroAvatarButton.onClick?.Invoke();
    }
}


