
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroWarehouseManager : MonoBehaviour {

    //private List<string> OwnedHeroes;
    
    [SerializeField]private List<Hero> OwnedHeroes;
    
    private Dictionary<string, Hero> ownedHeroesDic = new Dictionary<string, Hero>();

    
    public static HeroWarehouseManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        LoadListToDictionary();
    }
    
    ////////////////////////////////////Utils//////////////////////////////////////
    
    /// <summary>
    /// 将 OwnedHeroes 列表中的对象加载到字典中
    /// 使用 GameObject.name 作为 key
    /// </summary>
    private void LoadListToDictionary(){
        ownedHeroesDic.Clear(); // 清空旧数据，防止重复
        foreach (Hero hero in OwnedHeroes){
            if (hero == null) continue;
            string heroRef = hero.name;
            ownedHeroesDic[heroRef] = hero;
        }
    }
    
    /// <summary>
    /// 获取当前所有英雄 GameObject
    /// </summary>
    public List<string> GetOwnedHeroesRef(){
        return new List<string>(ownedHeroesDic.Keys);
    }

    public Sprite GetHeroSpriteByRef(string heroRef){
        Hero go = ownedHeroesDic[heroRef];
        return go.GetComponentInChildren<SpriteRenderer>().sprite;
    }
    
    public FighterType GetHeroType(string heroRef){
        Hero go = ownedHeroesDic[heroRef];
        return go.Type;
    }

    /// <summary>
    /// 根据 heroRef 获取对应的英雄 GameObject
    /// </summary>
    public Hero GetHeroByRef(string heroRef){
        return ownedHeroesDic.GetValueOrDefault(heroRef);
    }
    
    
    
    
}

