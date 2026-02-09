
using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroListSingle : MonoBehaviour {

    [SerializeField] private Image BorderImage;
    [SerializeField] private Button HeroAvatarButton;
    [SerializeField] private Image BloodBar;
    
    private Image HeroAvatarImage;
    public Action<string> OnClicked;
    private string CurrentHeroName;
    
    private void Awake() {
        this.HeroAvatarImage = this.HeroAvatarButton.GetComponent<Image>();
        this.HeroAvatarButton.onClick.AddListener(() => {
            this.OnClicked?.Invoke(this.CurrentHeroName);
        });
    }

    public void SetContent(Hero hero, Sprite borderSprite, Sprite heroAvatarSprite) {
        this.CurrentHeroName = hero ? hero.Name : null;
        this.BorderImage.sprite = borderSprite;
        this.HeroAvatarImage.sprite = heroAvatarSprite;
        this.HeroAvatarImage.color = new Color(1.0f, 1.0f, 1.0f, heroAvatarSprite ? 1.0f : 0.0f);
        this.HeroAvatarButton.enabled = hero;

        if (hero) {
            float health = SaveDataManager.Instance.GetHeroHealth(hero.Name);
            if (this.HeroAvatarImage.transform.childCount != 0) {
                this.HeroAvatarImage.transform.GetChild(0).gameObject.SetActive(health == 0.0f);
            }    
        }
        this.BloodBar.transform.parent.gameObject.SetActive(false);
        // this.SetHeroHealth(hero);
    }

    private void SetHeroHealth(Hero hero) {
        this.BloodBar.transform.parent.gameObject.SetActive(hero);
        if (!hero) return;
        float health = SaveDataManager.Instance.GetHeroHealth(hero.Name);
        this.BloodBar.fillAmount = health < 0.0f ? 1.0f : health / hero.InitialData.Health;
    }

    public void ClickButton() {
        this.HeroAvatarButton.onClick?.Invoke();
    }
}


