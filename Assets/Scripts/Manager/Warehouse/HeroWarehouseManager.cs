
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum HeroWarehouseCategory {
    All = -1,
    Warrior,
    Mage,
    Priest
}

public class HeroWarehouseManager : MonoBehaviour {
    
    [SerializeField] private List<Hero> AllHeroes;
    [SerializeField] private ModeHeroWarehouseUI ModeHeroWarehousePanel;
    
    private List<string> OwnedHeroes = new List<string>();
    
    private Dictionary<string, Hero> AllHeroMap = new Dictionary<string, Hero>();
    public static HeroWarehouseManager Instance;
    
    private Dictionary<string, int> HeroIndexMap = new Dictionary<string, int>();

    public int TotalHeroCount => this.AllHeroes.Count;
    public int OwnedHeroesCount => this.OwnedHeroes.Count;
    
    private CanvasGroup HeroWarehouseCanvasGroup;

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

        this.HeroWarehouseCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    public void TransitionHeroWarehouseCanvas(bool show) {
        if (show && this.HeroWarehouseCanvasGroup.alpha > 0.9f) return;
        this.HeroWarehouseCanvasGroup.alpha = show ? 1.0f : 0.0f;
        this.HeroWarehouseCanvasGroup.blocksRaycasts = show;
        this.HeroWarehouseCanvasGroup.interactable = show;
        if (show) this.ModeHeroWarehousePanel.Show();
        else this.ModeHeroWarehousePanel.Hide();
    }

    public int GetHeroIndex(string heroName) {
        return this.HeroIndexMap.GetValueOrDefault(heroName, -1);
    }

    private void Start() {
        SaveDataManager.Instance.OnLoadData += () => {
            this.OwnedHeroes = SaveDataManager.Instance.PlayerData.OwnedHeroes;
            if (this.OwnedHeroes.Count == 0) {
                this.AddHero(AllHeroes[0].Name);
            }
        };
    }

#if TEST_BATTLE
    public void TEMPFORBATTLE() {
        if (this.OwnedHeroes.Count == 0) {
            foreach (Hero hero in AllHeroes) {
                this.OwnedHeroes.Add(hero.Name);
            }
        }
    }
#endif
    
    public bool AddHero(string heroName) {
        if (!this.AllHeroMap.ContainsKey(heroName)) return false;
        if (!this.OwnedHeroes.Contains(heroName)) {
            this.OwnedHeroes.Add(heroName);
            return true;
        }
        return false;
    }

    public void RemoveHero(string heroName) {
        if (!this.OwnedHeroes.Contains(heroName)) return;
        this.OwnedHeroes.Remove(heroName);
    }

    ////////////////////////////////////Utils//////////////////////////////////////
    /// <summary>
    /// 获取当前所有英雄 GameObject
    /// </summary>
    public List<string> GetOwnedHeroesRef(){
        return OwnedHeroes;
    }

    public List<Hero> GetHeroesByType(HeroWarehouseCategory category = HeroWarehouseCategory.All) {
        List<Hero> result = new List<Hero>();
        foreach (string heroName in this.OwnedHeroes) {
            FighterType type = this.AllHeroMap[heroName].Type;
            if (category == HeroWarehouseCategory.All || (int)type == (int)category) {
                result.Add(this.AllHeroMap[heroName]);
            }
        }
        return result;
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

