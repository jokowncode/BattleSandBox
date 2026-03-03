
using System;
using System.Collections.Generic;
using UnityEngine;

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
        float changeValue = GetChangeValue();
        hero.FighterPropertyChange(Property, Property, ModifyWay, PropertyRef.Initial, changeValue, true);
        hero.Records.Add($"Hero{Property}PassiveEntryChangeValue", changeValue);
    }

    public override void Destruct(Hero hero) {
        if (!hero.Records.ContainsKey($"Hero{Property}PassiveEntryChangeValue")) return;
        float changeValue = (float) hero.Records[$"Hero{Property}PassiveEntryChangeValue"];
        hero.FighterPropertyChange(Property, Property, ModifyWay, PropertyRef.Initial, changeValue, false);
        hero.Records.Remove($"Hero{Property}PassiveEntryChangeValue");
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

