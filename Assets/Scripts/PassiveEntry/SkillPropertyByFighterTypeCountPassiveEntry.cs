
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
        float changeValue = GetChangeValue();
        hero.FighterSkillCaster.SkillPropertyChange(Property, ModifyWay, changeValue, true);
        hero.Records.Add($"Skill{Property}PassiveEntryChangeValue", changeValue);
    }

    public override void Destruct(Hero hero) {
        if (!hero.Records.ContainsKey($"Skill{Property}PassiveEntryChangeValue")) return;
        float changeValue = (float) hero.Records[$"Skill{Property}PassiveEntryChangeValue"];
        hero.FighterSkillCaster.SkillPropertyChange(Property, ModifyWay, changeValue, false);
        hero.Records.Remove($"Skill{Property}PassiveEntryChangeValue");
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

