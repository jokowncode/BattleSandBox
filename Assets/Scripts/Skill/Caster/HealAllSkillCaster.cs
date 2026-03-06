
using System.Collections.Generic;
using UnityEngine;

public class HealAllSkillCaster : SkillCaster{

    [SerializeField] private TargetType Type;

    public override bool CanCastSkill() {
        return base.CanCastSkill() && BattleManager.Instance.HasBeDamagedTarget(this.Type);
    }

    protected override void Cast(Transform attackTarget){
        if (Type == TargetType.Hero){
            List<Hero> heroes = BattleManager.Instance.HeroesInBattle;
            foreach (Hero hero in heroes){
                float value = GetSkillEffectValue(out bool isCritical);
                hero.BeHealed(new EffectData{
                    Value = value,
                    IsCritical = isCritical
                });
            }
        }else if (Type == TargetType.Enemy) {
            List<Enemy> enemies = BattleManager.Instance.EnemiesInBattle;
            foreach (Enemy enemy in enemies) {
                float value = GetSkillEffectValue(out bool isCritical);
                enemy.BeHealed(new EffectData{
                    Value = value,
                    IsCritical = isCritical
                });
            }
        }
    }
}

