
// TODO: Has AllHeroPassiveEntry Hero EnterField -> Construct; ExitField -> Destruct
using System.Collections.Generic;
using UnityEngine;

public class AllHeroPropertyPassiveEntry : PassiveEntry{

    [SerializeField] private FighterType TargetFighterType;
    [SerializeField] private FighterProperty Property;
    [SerializeField] private PropertyModifyWay ModifyWay;
    [SerializeField] private float Value;

    private void OnValidate(){
        if (ModifyWay == PropertyModifyWay.Percentage){
            Value = Mathf.Clamp(Value, -100.0f, 100.0f);
        }
    }

    public override void Construct(Hero _){
        List<Hero> heroes = BattleManager.Instance.HeroesInBattle;
        foreach (Hero hero in heroes){
            if(hero.Type == TargetFighterType) HeroStateUp(hero);
        }
        BattleManager.Instance.OnHeroEnterTheField += HeroStateUp;
        BattleManager.Instance.OnHeroExitTheField += HeroStateDown;
    }

    public override void Destruct(Hero _){
        List<Hero> heroes = BattleManager.Instance.HeroesInBattle;
        foreach (Hero hero in heroes){
            if(hero.Type == TargetFighterType) HeroStateDown(hero);
        }
        BattleManager.Instance.OnHeroEnterTheField -= HeroStateUp;
        BattleManager.Instance.OnHeroExitTheField -= HeroStateDown;
    }
    
    private void HeroStateUp(Hero hero){
        if(hero.Type == TargetFighterType) hero.HeroPropertyChange(Property, ModifyWay, Value, true);
    }

    private void HeroStateDown(Hero hero){
        if(hero.Type == TargetFighterType) hero.HeroPropertyChange(Property, ModifyWay, Value, false);
    }
}


