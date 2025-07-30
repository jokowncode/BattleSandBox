
using System.Collections.Generic;
using UnityEngine;

public class HealAllSkillCaster : SkillCaster{

    [SerializeField] private TargetType Type;
    
    protected override void Cast(Transform attackTarget){
        if (Type == TargetType.Hero){
            List<Hero> heroes = BattleManager.Instance.HeroesInBattle;
            foreach (Hero hero in heroes) {
                hero.BeHealed(new EffectData{
                    Value = GetSkillEffectValue()
                });
            }
        }else if (Type == TargetType.Enemy) {
            List<Enemy> enemies = BattleManager.Instance.EnemiesInBattle;
            foreach (Enemy enemy in enemies) {
                enemy.BeHealed(new EffectData{
                    Value = GetSkillEffectValue()
                });
            }
        }
    }
}

