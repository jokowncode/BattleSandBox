
using System.Collections.Generic;
using UnityEngine;

public class SkillPropertyByFighterTypeCountPassiveEntry : PassiveEntry {
    
    [SerializeField] private FighterType TargetFighterType;
    [SerializeField] private SkillProperty Property;
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
            SkillPropertyChangeUp(hero);
        }

        BattleManager.Instance.OnHeroEnterTheField += OnHeroEnterField;
        BattleManager.Instance.OnHeroExitTheField += OnHeroExitField;
    }

    private void SkillPropertyChangeUp(Hero currentHero){
        currentHero.FighterSkillCaster.SKillPropertyChange(Property, ModifyWay, CurrentChangeValue, true);
    }
    
    private void SkillPropertyChangeDown(Hero currentHero){
        currentHero.FighterSkillCaster.SKillPropertyChange(Property, ModifyWay, CurrentChangeValue, false);
    }

    private void OnHeroEnterField(Hero hero){
        if (hero == CurrentHero) return;
        if (hero.Type != TargetFighterType) return;
        SkillPropertyChangeDown(this.CurrentHero);
        if(hero.Type == TargetFighterType) Count += 1;
        CurrentChangeValue = Count * Value;
        SkillPropertyChangeUp(this.CurrentHero);
    }

    private void OnHeroExitField(Hero hero){
        if (hero == CurrentHero) return;
        if (hero.Type != TargetFighterType) return;
        SkillPropertyChangeDown(this.CurrentHero);
        if(hero.Type == TargetFighterType) Count -= 1;
        if (Count != 0){
            CurrentChangeValue = Count * Value;
            SkillPropertyChangeUp(this.CurrentHero);
        }
    }

    public override void Destruct(Hero hero){
        SkillPropertyChangeDown(hero);
        BattleManager.Instance.OnHeroEnterTheField -= OnHeroEnterField;
        BattleManager.Instance.OnHeroExitTheField -= OnHeroExitField;
    }
}

