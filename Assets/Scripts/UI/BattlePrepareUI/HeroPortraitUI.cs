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
    
    private Dictionary<string,HeroPanelUI> heroPortraitUIDict;
    
    private void Awake() {
        heroPortraitUIDict = new Dictionary<string,HeroPanelUI>();
    }

    public void CreateUIProtraits(List<Hero> heroes){
        foreach (Transform child in heroPortraitContent.transform){
            Destroy(child.gameObject);
        }
        // 清空旧字典
        heroPortraitUIDict.Clear();
        foreach (Hero hero in heroes){
            FighterType tempType = hero.Type;
            HeroPanelUI go;
            if(tempType == FighterType.Warrior)
                go = Instantiate(heroWarriorPortraitUIPrefab, heroPortraitContent);
            else if(tempType == FighterType.Mage)
                go =  Instantiate(heroMagePortraitUIPrefab, heroPortraitContent);
            else
                go=Instantiate(heroPriestPortraitUIPrefab, heroPortraitContent);
            
            go.SetPortrait(hero.WarehouseHeroPortrait, true);
            if(!go.TryGetComponent(out UISelectableShaker uiNode)) {
                uiNode = go.AddComponent<UISelectableShaker>();
            }

            uiNode.CurrentHero = hero;
            heroPortraitUIDict.Add(hero.Name, go);
        }
        heroPortraitContent.GetComponent<UILayoutManual>().LayoutChildren();
    }

    public void SetHeroPortraitsGray(Hero hero){
        if (!heroPortraitUIDict.ContainsKey(hero.Name)) return;
        if (heroPortraitUIDict[hero.Name].TryGetComponent(out UISelectableShaker shaker))
        {
            shaker.SetDead();
            shaker.Shake();
        }
        heroPortraitUIDict[hero.Name].HeroDead();
    }

    public void SetHeroEnergy(Hero hero, float value) {
        if (heroPortraitUIDict.ContainsKey(hero.Name)) {
            heroPortraitUIDict[hero.Name].SetHeroEnergy(value);
        }
    }

    public bool HeroEnergyIsFull(string heroName) {
        if (heroPortraitUIDict.ContainsKey(heroName)) {
            return heroPortraitUIDict[heroName].EnergyIsFull;
        }
        return false;
    }

    public void SelectOneHero(string heroName) {
        List<string> hasTacticHero = EntanglementManager.Instance.GetHasTacticHeroNames(heroName);
        foreach (string otherHeroName in hasTacticHero) {
            if (this.heroPortraitUIDict.TryGetValue(otherHeroName, out HeroPanelUI heroPanelUI)
                && heroPanelUI.TryGetComponent(out UISelectableShaker shaker)) {
                shaker.HasEntanglement();
            }
        }
    }

    public void DownAllPanel(bool onlyHasTactic) {
        foreach (var pair in heroPortraitUIDict) {
            if (pair.Value.TryGetComponent(out UISelectableShaker shaker)) {
                if (!onlyHasTactic || (shaker.HasTactic && !shaker.IsSelected)) {
                    shaker.GoDown(true);                    
                }
            }
        }
    }
}
