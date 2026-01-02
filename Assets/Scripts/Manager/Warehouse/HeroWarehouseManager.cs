
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroWarehouseManager : MonoBehaviour {
    
    [SerializeField] private List<Hero> AllHeroes;
    
    private List<string> OwnedHeroes = new List<string>();
    
    private Dictionary<string, Hero> AllHeroMap = new Dictionary<string, Hero>();
    public static HeroWarehouseManager Instance;
    
    private Dictionary<string, int> HeroIndexMap = new Dictionary<string, int>();

    public int TotalHeroCount => this.AllHeroes.Count;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        for (int i = 0; i < this.AllHeroes.Count; i++) {
            Hero hero = this.AllHeroes[i];
            this.AllHeroMap.Add(hero.Name, hero);
            this.HeroIndexMap.Add(hero.Name, i);
        }
    }

    public int GetHeroIndex(string heroName) {
        return this.HeroIndexMap.GetValueOrDefault(heroName, -1);
    }

    private void Start() {
        SaveMapManager.Instance.OnSaveData += () => {
            string json = JsonUtility.ToJson(new Serialization<string>(this.OwnedHeroes));
            PlayerPrefs.SetString("OwnedHeroWarehouse", json);
        };

        SaveMapManager.Instance.OnLoadData += () => {
            this.OwnedHeroes.Clear();
            // TODO: TEMP Debug Battle
            /*if (PlayerPrefs.HasKey("OwnedHeroWarehouse")) {
                string json = PlayerPrefs.GetString("OwnedHeroWarehouse");
                this.OwnedHeroes = JsonUtility.FromJson<Serialization<string>>(json).ToList();
            } else {
                this.OwnedHeroes.Add(this.AllHeroes[0].Name);
            }*/
            
            // TODO: TEMP Debug Battle
            foreach (Hero hero in AllHeroes) {
                this.OwnedHeroes.Add(hero.Name);
            }    
        };
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

