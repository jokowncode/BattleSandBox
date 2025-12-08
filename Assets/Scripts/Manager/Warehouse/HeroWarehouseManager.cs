
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroWarehouseManager : MonoBehaviour {
    
    [SerializeField] private List<Hero> AllHeroes;
    
    private List<string> OwnedHeroes = new List<string>();
    
    private Dictionary<string, Hero> AllHeroMap = new Dictionary<string, Hero>();
    public static HeroWarehouseManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        foreach (Hero hero in AllHeroes) {
            this.AllHeroMap.Add(hero.Name, hero);
        }
        
        // TODO: TEMP Debug Battle
        foreach (Hero hero in AllHeroes) {
            this.OwnedHeroes.Add(hero.Name);
        }
    }

    private void Start() {
        // TODO: TEMP Debug Battle
        /*if (PlayerPrefs.HasKey("OwnedHeroWarehouse")) {
            string json = PlayerPrefs.GetString("OwnedHeroWarehouse");
            this.OwnedHeroes = JsonUtility.FromJson<Serialization<string>>(json).ToList();
        } else {
            this.OwnedHeroes.Add(this.AllHeroes[0].Name);
        }*/
    }

    private void OnDestroy() {
        // TODO: TEMP Debug Battle
        /*string json = JsonUtility.ToJson(new Serialization<string>(this.OwnedHeroes));
        PlayerPrefs.SetString("OwnedHeroWarehouse", json);*/
    }

    public void AddHero(string heroName) {
        this.OwnedHeroes.Add(heroName);
    }

    ////////////////////////////////////Utils//////////////////////////////////////
    /// <summary>
    /// 获取当前所有英雄 GameObject
    /// </summary>
    public List<string> GetOwnedHeroesRef(){
        return OwnedHeroes;
    }

    public Sprite GetHeroSpriteByRef(string heroRef){
        Hero go = AllHeroMap[heroRef];
        return go.GetComponentInChildren<SpriteRenderer>().sprite;
    }
    
    public FighterType GetHeroType(string heroRef){
        Hero go = AllHeroMap[heroRef];
        return go.Type;
    }

    /// <summary>
    /// 根据 heroRef 获取对应的英雄 GameObject
    /// </summary>
    public Hero GetHeroByRef(string heroRef){
        return AllHeroMap.GetValueOrDefault(heroRef);
    }
    
    
    
    
}

