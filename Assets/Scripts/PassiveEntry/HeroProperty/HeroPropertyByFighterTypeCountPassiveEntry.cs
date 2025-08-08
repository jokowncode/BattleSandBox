
using System.Collections.Generic;
using UnityEngine;

public class HeroPropertyByFighterTypeCountPassiveEntry : PassiveEntry {
    
    [SerializeField] private FighterType TargetFighterType;
    [SerializeField] private FighterProperty Property;
    [SerializeField] private PropertyModifyWay ModifyWay;
    [SerializeField] private float Value;

    private int Count;
    private float CurrentChangeValue;
    private Hero CurrentHero;
    
    private void OnValidate(){
        if (ModifyWay == PropertyModifyWay.Percentage){
            Value = Mathf.Clamp(Value, -100.0f, 100.0f);
        }
    }
    
    public override void Construct(Hero hero){
        this.CurrentHero = hero;
        List<Hero> heroes = BattleManager.Instance.HeroesInBattle;
        foreach (Hero inBattleHero in heroes){
            if(inBattleHero.Type == TargetFighterType) Count += 1;
        }

        if (Count != 0){
            CurrentChangeValue = Count * Value;
            HeroPropertyChangeUp(hero);
        }

        BattleManager.Instance.OnHeroEnterTheField += OnHeroEnterField;
        BattleManager.Instance.OnHeroExitTheField += OnHeroExitField;
    }

    private void HeroPropertyChangeUp(Hero currentHero){
        currentHero.FighterPropertyChange(Property, ModifyWay, CurrentChangeValue, true);
    }
    
    private void HeroPropertyChangeDown(Hero currentHero){
        currentHero.FighterPropertyChange(Property, ModifyWay, CurrentChangeValue, false);
    }

    private void OnHeroEnterField(Hero hero){
        if (hero == CurrentHero) return;
        if (hero.Type != TargetFighterType) return;
        HeroPropertyChangeDown(this.CurrentHero);
        if(hero.Type == TargetFighterType) Count += 1;
        CurrentChangeValue = Count * Value;
        HeroPropertyChangeUp(this.CurrentHero);
    }

    private void OnHeroExitField(Hero hero){
        if (hero == CurrentHero) return;
        if (hero.Type != TargetFighterType) return;
        HeroPropertyChangeDown(this.CurrentHero);
        if(hero.Type == TargetFighterType) Count -= 1;
        if (Count != 0){
            CurrentChangeValue = Count * Value;
            HeroPropertyChangeUp(this.CurrentHero);
        }
    }

    public override void Destruct(Hero hero){
        HeroPropertyChangeDown(hero);
        BattleManager.Instance.OnHeroEnterTheField -= OnHeroEnterField;
        BattleManager.Instance.OnHeroExitTheField -= OnHeroExitField;
    }
}

