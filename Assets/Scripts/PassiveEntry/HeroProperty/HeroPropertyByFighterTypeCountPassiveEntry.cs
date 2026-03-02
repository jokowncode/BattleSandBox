
using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: BUG -> IF ENTER HERO SATISFY CONDITION, BUT NOT OPEN THIS HERO DETAIL -> PROPERTY NOT CHANGE
public class HeroPropertyByFighterTypeCountPassiveEntry : PassiveEntry {
    
    [SerializeField] private FighterType TargetFighterType;
    [SerializeField] private FighterProperty Property;
    [SerializeField] private PropertyModifyWay ModifyWay;
    [SerializeField] private float Value;

    private void OnValidate(){
        if (ModifyWay == PropertyModifyWay.Percentage){
            Value = Mathf.Clamp(Value, -100.0f, 100.0f);
        }
    }
    
    public override void Construct(Hero hero){
        hero.OnShowHeroDetail += OnShowHeroDetail;
        float changeValue = GetChangeValue();
        hero.FighterPropertyChange(Property, Property, ModifyWay, PropertyRef.Initial, changeValue, true);
        hero.Records.Add($"Hero{Property}PassiveEntryChangeValue", changeValue);
    }

    private void OnShowHeroDetail(Hero hero) {
        float changeValue = GetChangeValue();
        string key = $"Hero{Property}PassiveEntryChangeValue";
        if (hero.Records.ContainsKey(key)) {
            hero.FighterPropertyChange(Property, Property, ModifyWay, PropertyRef.Initial, (float)hero.Records[key], false);
        }
        hero.FighterPropertyChange(Property, Property, ModifyWay, PropertyRef.Initial, changeValue, true);
        hero.Records[key] = changeValue;
    }

    public override void Destruct(Hero hero) {
        hero.OnShowHeroDetail -= OnShowHeroDetail;
        float changeValue = GetChangeValue();
        hero.FighterPropertyChange(Property, Property, ModifyWay, PropertyRef.Initial, changeValue, false);
    }

    private float GetChangeValue() {
        List<Hero> heroes = BattleManager.Instance.HeroesInBattle;
        int count = 0;
        foreach (Hero hero in heroes) {
            if (hero.Type == this.TargetFighterType) count += 1;
        }
        return count * this.Value;
    }
}

