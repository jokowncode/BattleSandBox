
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroWarehouseManager : MonoBehaviour {

    //private List<string> OwnedHeroes;
    
    [SerializeField]private List<GameObject> OwnedHeroes;
    
    private Dictionary<string, GameObject> ownedHeroes = new Dictionary<string, GameObject>();

    
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

    // TODO
    public void AddHero(string heroRef) { }
    public void RemoveHero(string heroRef) { }

    //public List<string> GetOwnedHeroes() { return this.OwnedHeroes; }
    public List<GameObject> GetOwnedHeroes() { return this.OwnedHeroes; }
    
    ////////////////////////////////////Utils//////////////////////////////////////
    
    /// <summary>
    /// 将 OwnedHeroes 列表中的对象加载到字典中
    /// 使用 GameObject.name 作为 key
    /// </summary>
    public void LoadListToDictionary()
    {
        ownedHeroes.Clear(); // 清空旧数据，防止重复

        foreach (GameObject hero in OwnedHeroes)
        {
            if (hero == null) continue;

            string heroRef = hero.name;

            if (!ownedHeroes.ContainsKey(heroRef))
            {
                ownedHeroes.Add(heroRef, hero);
            }
            else
            {
                Debug.LogWarning($"重复的 heroRef: {heroRef}，已跳过");
            }
        }
    }
    
    /// <summary>
    /// 获取当前所有英雄 GameObject
    /// </summary>
    public List<string> GetOwnedHeroesRef()
    {
        return new List<string>(ownedHeroes.Keys);
    }

    public Sprite GetHeroSpriteByRef(string heroRef)
    {
        GameObject go = ownedHeroes[heroRef];
        return go.GetComponentInChildren<SpriteRenderer>().sprite;
    }
    
    public FighterType GetHeroType(string heroRef)
    {
        GameObject go = ownedHeroes[heroRef];
        return go.GetComponent<Hero>().Type;
    }

    /// <summary>
    /// 根据 heroRef 获取对应的英雄 GameObject
    /// </summary>
    public GameObject GetHeroByRef(string heroRef)
    {
        if (ownedHeroes.TryGetValue(heroRef, out GameObject hero))
        {
            return hero;
        }
        return null;
    }
    
    
    
    
}

