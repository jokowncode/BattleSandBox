using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeroPortraitUI : MonoBehaviour {
    
    public GameObject heroPriestPortraitUIPrefab;
    public GameObject heroWarriorPortraitUIPrefab;
    public GameObject heroMagePortraitUIPrefab;
    public Transform heroPortraitContent;
    
    [SerializeField] private List<Hero> heroPortraits;
    private Dictionary<Hero,GameObject> heroPortraitUIDict;
    
    private void Awake() {
        heroPortraitUIDict = new Dictionary<Hero,GameObject>();
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
            FighterType tempType = hero.GetFighterData().Type;
            GameObject go;
            if(tempType == FighterType.Warrior)
                go = Instantiate(heroWarriorPortraitUIPrefab, heroPortraitContent);
            else if(tempType == FighterType.Mage)
                go =  Instantiate(heroMagePortraitUIPrefab, heroPortraitContent);
            else
                go=Instantiate(heroPriestPortraitUIPrefab, heroPortraitContent);

            Image[] imageComponent = go.GetComponentsInChildren<Image>();
            imageComponent[3].sprite = hero.GetFighterData().heroPortraitSprite;
            heroPortraitUIDict.Add(hero,go);
        }
        heroPortraitContent.GetComponent<UILayoutManual>().LayoutChildren();
    }

    public void SetHeroPortraitsGray(Hero hero){
        if (heroPortraitUIDict[hero].TryGetComponent(out UIShaker shaker)){
            shaker.Shake();
        }
        
        Image[] image = heroPortraitUIDict[hero].GetComponentsInChildren<Image>();
        Material newMat = new Material(image[3].material);
        newMat.SetFloat(MaterialProperty.Desaturation, 0);
        image[3].material = newMat;
        image[3].color = Color.gray;
    }
    
}
