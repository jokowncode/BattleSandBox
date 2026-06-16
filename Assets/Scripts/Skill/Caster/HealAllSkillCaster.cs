
using System.Collections.Generic;
using UnityEngine;

public class HealAllSkillCaster : SkillCaster{

    [SerializeField] protected TargetType Type;

    public override bool CanCastSkill() {
        return base.CanCastSkill() && BattleFindCharacterTools.HasBeDamagedTarget(this.Type);
    }

    protected override void Cast(Transform attackTarget){
        List<Fighter> fighters = new List<Fighter>(Type == TargetType.Hero ? BattleManager.Instance.HeroesInBattle : BattleManager.Instance.EnemiesInBattle);
        foreach (Fighter fighter in fighters){
            float value = GetSkillEffectValue(out bool isCritical);
            fighter.BeHealed(new EffectData{
                Value = value,
                IsCritical = isCritical
            });
        }
    }
}

