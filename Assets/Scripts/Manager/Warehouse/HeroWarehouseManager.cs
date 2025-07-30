
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroWarehouseManager : MonoBehaviour {

    //private List<string> OwnedHeroes;
    
    public List<GameObject> OwnedHeroes;
    
    public static HeroWarehouseManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // TODO
    public void AddHero(string heroRef) { }
    public void RemoveHero(string heroRef) { }

    //public List<string> GetOwnedHeroes() { return this.OwnedHeroes; }
    public List<GameObject> GetOwnedHeroes() { return this.OwnedHeroes; }
}

