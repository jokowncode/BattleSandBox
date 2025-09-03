using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllHeroBuffSkillStart : SkillStart {
    
    public BuffData BuffData;
    
    public override void AdditionalProcedure(GameObject target, float damage,Fighter ownedFighter, int count) {
        foreach(Hero hero in BattleManager.Instance.HeroesInBattle) {
            BuffManager.Instance.AddBuff(ownedFighter, hero, BuffData);
        }
    }
}
