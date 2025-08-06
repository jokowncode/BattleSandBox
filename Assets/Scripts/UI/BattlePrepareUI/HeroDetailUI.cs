using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HeroDetailUI : MonoBehaviour {
    
    [SerializeField] private Image heroImage;
    
    [Header("Detail")]
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private GameObject StarLevelUI;
    [SerializeField] private GameObject StarLevelPrefab;
    [SerializeField] private TextMeshProUGUI Hp;
    [SerializeField] private TextMeshProUGUI PhysicsAttack;
    [SerializeField] private TextMeshProUGUI MagicAttack;
    [SerializeField] private TextMeshProUGUI Speed;
    [SerializeField] private TextMeshProUGUI Critical;
    [SerializeField] private TextMeshProUGUI Cooldown;

    public void Hide(){
        this.gameObject.SetActive(false);
    }

    public void ChangeHeroDetailUIValue(Sprite sprite){
        heroImage.sprite = sprite;
    }

    public void ChangeDetailUI(Hero hero){
        Name.text = hero.Name;
        Description.text = hero.Description;
        UpdateStarLevelUI(hero);
        Hp.text = hero.Health.ToString();
        PhysicsAttack.text = hero.PhysicsAttack.ToString();
        MagicAttack.text = hero.MagicAttack.ToString();
        Speed.text = hero.Speed.ToString();
        Critical.text = hero.Critical.ToString();
        Cooldown.text = hero.FighterSkillCaster.Data.Cooldown.ToString();
    }

    private void UpdateStarLevelUI(Hero hero){
        foreach (Transform child in StarLevelUI.transform){
            Destroy(child.gameObject);
        }

        for (int i = 0; i < hero.StarLevel; i++){
            GameObject go = Instantiate(StarLevelPrefab, StarLevelUI.transform);
        }
        
    }
    
    
}
