using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class SkillWarehouseUI : WarehouseUI {

    public void UpdateHeroWarehouse() {
        
        ClearWarehouse();
        
        // List<string> ownedHeroes = HeroWarehouseManager.Instance.GetOwnedHeroes();
        // foreach (string heroRef in ownedHeroes) {
        //     Hero hero = AssetManager.Instance.LoadAsset<Hero>(heroRef);
        //     // TODO: Update Hero Warehouse UI
        //     
        // }
        List<SkillData> ownedHeroes = SkillWarehouseManager.Instance.GetOwnedHeroes();
        foreach (SkillData skillData in ownedHeroes)
        {
            // TODO: Update Hero Warehouse UI
            AddItem(skillData);
        }
    }

    public override void AddItem(SkillData skillData)
    {
        GameObject go = Instantiate(warehouseImageUIPrefab, warehouseContent);
        go.GetComponentInChildren<ClickableUI>().skillData = skillData;
    }
    
    
    
    
}
