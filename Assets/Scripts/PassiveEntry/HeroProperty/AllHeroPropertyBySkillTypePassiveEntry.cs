
using System.Collections.Generic;
using UnityEngine;

public class AllHeroPropertyBySkillTypePassiveEntry : PassiveEntry {
    
    [SerializeField] private PassiveEntrySort TargetSkillSort;
    [SerializeField] private FighterProperty Property;
    [SerializeField] private PropertyModifyWay ModifyWay;
    [SerializeField] private float Value;

    private void OnValidate(){
        if (ModifyWay == PropertyModifyWay.Percentage){
            Value = Mathf.Clamp(Value, -100.0f, 100.0f);
        }
    }

    private bool Valid(Hero hero) {
        return hero.FighterSkillCaster && hero.FighterSkillCaster.Sort == this.TargetSkillSort;
    }

    public override void Construct(Hero _){
        List<Hero> heroes = BattleManager.Instance.HeroesInBattle;
        foreach (Hero hero in heroes){
            if(Valid(hero)) HeroStateUp(hero);
        }
        BattleManager.Instance.OnHeroEnterTheField += HeroStateUp;
        BattleManager.Instance.OnHeroExitTheField += HeroStateDown;
    }

    public override void Destruct(Hero _){
        List<Hero> heroes = BattleManager.Instance.HeroesInBattle;
        foreach (Hero hero in heroes){
            if(Valid(hero)) HeroStateDown(hero);
        }
        BattleManager.Instance.OnHeroEnterTheField -= HeroStateUp;
        BattleManager.Instance.OnHeroExitTheField -= HeroStateDown;
    }
    
    private void HeroStateUp(Hero hero){
        if(Valid(hero)) hero.FighterPropertyChange(Property, Property, ModifyWay, PropertyRef.Initial, Value, true);
    }

    private void HeroStateDown(Hero hero){
        if(Valid(hero)) hero.FighterPropertyChange(Property, Property, ModifyWay, PropertyRef.Initial, Value, false);
    }
}


