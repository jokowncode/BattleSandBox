using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeroPortraitUI : MonoBehaviour
{
    [HideInInspector]public static HeroPortraitUI Instance;
    
    public GameObject heroPriestPortraitUIPrefab;
    public GameObject heroWarriorPortraitUIPrefab;
    public GameObject heroMagePortraitUIPrefab;
    public Transform heroPortraitContent;
    
    [SerializeField] private List<Hero> heroPortraits;
    [SerializeField] private Dictionary<Hero,GameObject> heroPortraitUIDict;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        heroPortraitUIDict = new Dictionary<Hero,GameObject>();
    }

    // void Start()
    // {
    //     heroPortraitUIDict = new Dictionary<Hero,GameObject>();
    //     //CreateUIProtraits();
    // }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("wow");
            SetHeroPortraitsGray(heroPortraits[0]);
            //DoSomething(); // 👈 你要执行的函数
        }
    }
    public void PushHeros(List<Hero> heroes)
    {
        heroPortraits.Clear();
        heroPortraits.AddRange(heroes);
        //heroPortraits = heroes;
        CreateUIProtraits();
    }

    void CreateUIProtraits()
    {
        foreach (Transform child in heroPortraitContent.transform)
        {
            Destroy(child.gameObject);
        }
        // 清空旧字典
        if(heroPortraitUIDict!=null)
            heroPortraitUIDict.Clear();
        foreach (Hero hero in heroPortraits)
        {
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

    public void SetHeroPortraitsGray(Hero hero)
    {
        heroPortraitUIDict[hero].GetComponent<UIShaker>().Shake();
        Image[] image = heroPortraitUIDict[hero].GetComponentsInChildren<Image>();
        image[3].material.SetFloat(Shader.PropertyToID("_Desaturation"),0);
        image[3].color = Color.gray;
    }
    
}
