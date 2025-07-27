using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroWarehouseUI : MonoBehaviour {

    public void UpdateHeroWarehouse() {
        List<string> ownedHeroes = HeroWarehouseManager.Instance.GetOwnedHeroes();
        foreach (string heroRef in ownedHeroes) {
            Hero hero = AssetManager.Instance.LoadAsset<Hero>(heroRef);
            // TODO: Update Hero Warehouse UI
        }
    }

}
