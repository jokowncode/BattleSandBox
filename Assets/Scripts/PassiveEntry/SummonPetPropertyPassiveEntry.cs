
using System.Collections.Generic;
using UnityEngine;

public class SummonPetPropertyPassiveEntry : PassiveEntry {
    
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
            if (hero.FighterSkillCaster && hero.FighterSkillCaster is SummonSkillCaster summonSkillCaster) {
                summonSkillCaster.OnSummon += OnSummon;
            }
        }
        BattleManager.Instance.OnHeroEnterTheField += OnHeroEnterTheField;
        BattleManager.Instance.OnHeroExitTheField += OnHeroExitTheField;
    }

    private void OnHeroExitTheField(Hero hero) {
        if (hero.FighterSkillCaster && hero.FighterSkillCaster is SummonSkillCaster summonSkillCaster) {
            summonSkillCaster.OnSummon -= OnSummon;
        }
    }

    private void OnHeroEnterTheField(Hero hero) {
        if (hero.FighterSkillCaster && hero.FighterSkillCaster is SummonSkillCaster summonSkillCaster) {
            summonSkillCaster.OnSummon += OnSummon;
        }
    }

    private void OnSummon(Fighter summonFighter) {
        // TODO: If Passive Entry Destruct, Already summon pet not rewind Change Property
        summonFighter.FighterPropertyChange(this.Property, this.Property, this.ModifyWay, PropertyRef.Initial,
            this.Value, true);
    }

    public override void Destruct(Hero _){
        List<Hero> heroes = BattleManager.Instance.HeroesInBattle;
        foreach (Hero hero in heroes){
            if (hero.FighterSkillCaster && hero.FighterSkillCaster is SummonSkillCaster summonSkillCaster) {
                summonSkillCaster.OnSummon -= OnSummon;
            }
        }
        BattleManager.Instance.OnHeroEnterTheField -= OnHeroEnterTheField;
        BattleManager.Instance.OnHeroExitTheField -= OnHeroExitTheField;
    }
}

