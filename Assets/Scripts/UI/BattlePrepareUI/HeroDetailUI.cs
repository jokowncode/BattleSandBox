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
    
    [SerializeField] private TextMeshProUGUI HpChange;
    [SerializeField] private TextMeshProUGUI PhysicsAttackChange;
    [SerializeField] private TextMeshProUGUI MagicAttackChange;
    [SerializeField] private TextMeshProUGUI CriticalChange;
    [SerializeField] private TextMeshProUGUI CooldownChange;

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
        UpdateDetailUI(hero);
    }

    private string GetPropertyDiff(float current, float initial){
        float diff = current - initial;
        if (diff == 0){
            return "";
        }
        string sign = diff > 0 ? "+" : "";
        return sign + diff;
    }

    public void UpdateDetailUI(Hero hero){
        HpChange.text = GetPropertyDiff(hero.Health, hero.InitialHealth);
        PhysicsAttackChange.text = GetPropertyDiff(hero.PhysicsAttack, hero.InitialPhysicsAttack);
        MagicAttackChange.text = GetPropertyDiff(hero.MagicAttack, hero.InitialMagicAttack);
        CriticalChange.text = GetPropertyDiff(hero.Critical, hero.InitialCritical);
        CooldownChange.text = GetPropertyDiff(hero.FighterSkillCaster.GetCurrentData(SkillProperty.Cooldown),
            hero.FighterSkillCaster.GetInitialData(SkillProperty.Cooldown));
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
