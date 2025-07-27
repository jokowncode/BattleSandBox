
using System;
using UnityEngine;

public class BattleUIManager : MonoBehaviour {

    [SerializeField] private HeroWarehouseUI WarehouseUI;

    private void Awake() {
        WarehouseUI.UpdateHeroWarehouse();
    }
}

