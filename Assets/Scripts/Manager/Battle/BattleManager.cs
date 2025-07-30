using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : StateMachineController {

    public static BattleManager Instance;
    
    // TODO: Get BattleData From World Scene
    [SerializeField] private BattleData Data;
    [SerializeField] private EnemyDepartmentArea EnemyArea;
    
    //private List<Hero> HeroesInBattle;
    public List<GameObject> HeroesInBattle;
    private List<Enemy> EnemiesInBattle;

    // TODO: eg:Support Passive Entry Register Action to Change Hero Property
    public Action OnHeroEnterTheField;
    public Action OnHeroExitTheField;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        HeroesInBattle = new List<GameObject>();
        // EnemiesInBattle = EnemyArea.InitializeEnemy(Data.EnemiesInBattle);
        // HeroesInBattle = new List<Hero>();
        //
        // // Turn To Prepare State
        // ChangeState(GetComponent<PrepareState>());
    }

    public void StartBattle()
    {
        foreach (GameObject hero in HeroesInBattle)
        {
            hero.GetComponent<Hero>().BattleStart();
        }
    }

    public void AddHero(GameObject hero)
    {
        HeroesInBattle.Add(hero);
    }
    
    
}

