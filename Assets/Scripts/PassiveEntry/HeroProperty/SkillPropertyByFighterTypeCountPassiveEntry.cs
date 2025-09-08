
using System.Collections.Generic;
using UnityEngine;

public class SkillPropertyByFighterTypeCountPassiveEntry : PassiveEntry {
    
    [SerializeField] private FighterType TargetFighterType;
    [SerializeField] private SkillProperty Property;
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
        hero.FighterSkillCaster.SKillPropertyChange(Property, ModifyWay, changeValue, true);
        hero.Records.Add($"Skill{Property}PassiveEntryChangeValue", changeValue);
    }

    private void OnShowHeroDetail(Hero hero) {
        float changeValue = GetChangeValue();
        string key = $"Skill{Property}PassiveEntryChangeValue";
        if (hero.Records.ContainsKey(key)) {
            hero.FighterSkillCaster.SKillPropertyChange(Property, ModifyWay, (float)hero.Records[key], false);
        }
        hero.FighterSkillCaster.SKillPropertyChange(Property, ModifyWay, changeValue, true);
        hero.Records[key] = changeValue;
    }

    public override void Destruct(Hero hero) {
        hero.OnShowHeroDetail -= OnShowHeroDetail;
        float changeValue = GetChangeValue();
        hero.FighterSkillCaster.SKillPropertyChange(Property, ModifyWay, changeValue, false);
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

