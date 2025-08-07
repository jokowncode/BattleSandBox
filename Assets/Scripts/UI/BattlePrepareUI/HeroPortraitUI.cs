using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeroPortraitUI : MonoBehaviour {
    
    public HeroPanelUI heroPriestPortraitUIPrefab;
    public HeroPanelUI heroWarriorPortraitUIPrefab;
    public HeroPanelUI heroMagePortraitUIPrefab;
    public Transform heroPortraitContent;
    
    [SerializeField] private List<Hero> heroPortraits;
    private Dictionary<Hero,HeroPanelUI> heroPortraitUIDict;
    
    private void Awake() {
        heroPortraitUIDict = new Dictionary<Hero,HeroPanelUI>();
    }
    
    
    public void PushHeros(List<Hero> heroes){
        heroPortraits.Clear();
        heroPortraits.AddRange(heroes);
        //heroPortraits = heroes;
        CreateUIProtraits();
    }

    private void CreateUIProtraits(){
        foreach (Transform child in heroPortraitContent.transform){
            Destroy(child.gameObject);
        }
        // 清空旧字典
        heroPortraitUIDict.Clear();
        foreach (Hero hero in heroPortraits){
            FighterType tempType = hero.Type;
            HeroPanelUI go;
            if(tempType == FighterType.Warrior)
                go = Instantiate(heroWarriorPortraitUIPrefab, heroPortraitContent);
            else if(tempType == FighterType.Mage)
                go =  Instantiate(heroMagePortraitUIPrefab, heroPortraitContent);
            else
                go=Instantiate(heroPriestPortraitUIPrefab, heroPortraitContent);
            
            go.SetPortrait(hero.heroPortraitSprite);
            heroPortraitUIDict.Add(hero,go);
        }
        heroPortraitContent.GetComponent<UILayoutManual>().LayoutChildren();
    }

    public void SetHeroPortraitsGray(Hero hero){
        if (!heroPortraitUIDict.ContainsKey(hero)) return;
        if (heroPortraitUIDict[hero].TryGetComponent(out UIShaker shaker)){
            shaker.Shake();
        }
        heroPortraitUIDict[hero].HeroDead();
    }
    
}
