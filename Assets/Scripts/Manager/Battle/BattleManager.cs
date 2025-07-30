using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : StateMachineController {

    public static BattleManager Instance;
    
    // TODO: Get BattleData From World Scene
    [SerializeField] private BattleData Data;
    [SerializeField] private EnemyDepartmentArea EnemyArea;

    // TODO: Get From Department
    [field: SerializeField] public List<Hero> HeroesInBattle{ get; private set; }
    public List<Enemy> EnemiesInBattle{ get; private set; }

    // TODO: eg:Support Passive Entry Register Action to Change Hero Property
    public Action OnHeroEnterTheField;
    public Action OnHeroExitTheField;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        
        // TODO: Initiate Enemy
        // EnemiesInBattle = EnemyArea.InitializeEnemy(Data.EnemiesInBattle);
        // HeroesInBattle = new List<Hero>();
        
        // Turn To Prepare State
        ChangeState(GetComponent<PrepareState>());
    }

    public Fighter FindMinPercentagePropertyHero(FighterProperty property){
        Fighter result = null;
        float minPercentage = 1.0f;
        foreach (Hero hero in HeroesInBattle){
            float currentValue = ReflectionTools.GetObjectProperty<float>(property.ToString(), hero);
            float initialValue = ReflectionTools.GetObjectProperty<float>("Initial"+property, hero);
            float percentage = currentValue / initialValue;
            if (percentage < minPercentage){
                minPercentage = percentage;
                result = hero;
            }
        }
        return result;
    }

}

