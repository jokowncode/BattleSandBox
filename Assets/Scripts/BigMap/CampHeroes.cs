

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CampHeroes : MonoBehaviour {
    private void Start() {
        List<string> ownedHeroes = HeroWarehouseManager.Instance.GetOwnedHeroesRef();
        List<string> tmp = new List<string>();
        foreach (string heroName in ownedHeroes) {
            if (heroName == "Elara") continue;
            tmp.Add(heroName);
        }

        if (tmp.Count == 0) return;
        foreach (Transform child in this.transform) {
            int randomIndex = Random.Range(0, tmp.Count);
            Hero hero = HeroWarehouseManager.Instance.GetHeroByRef(tmp[randomIndex]);
            Instantiate(hero.WarehouseData.CampHeroPrefab, child);
            tmp.RemoveAt(randomIndex);
            if (tmp.Count == 0) break;
        }
    }
}



