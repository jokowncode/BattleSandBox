
using System;
using UnityEngine;

public class BattleUIManager : MonoBehaviour {

    [SerializeField] private HeroWarehouseUI heroWarehouseUI;
    [SerializeField] private SkillWarehouseUI skillWarehouseUI;

    private void Start() {
        heroWarehouseUI.UpdateHeroWarehouse();
        //skillWarehouseUI.UpdateHeroWarehouse();
    }
}

