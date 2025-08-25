
using UnityEngine;

public class AllHeroBuffSkillCaster : AddBuffSkillCaster {
    protected override void Cast(Transform attackTarget) {
        foreach(Hero hero in BattleManager.Instance.HeroesInBattle) {
            AddBuff(hero);
        }
    }
}

