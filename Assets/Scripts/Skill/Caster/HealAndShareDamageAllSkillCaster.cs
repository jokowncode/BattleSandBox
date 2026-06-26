
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAndShareDamageAllSkillCaster : HealAllSkillCaster{

    private bool IsInDamageShare = false;
    private WaitForSeconds Timer;

    protected override void Awake(){
        base.Awake();
        this.Timer = new WaitForSeconds(this.Data.Duration);
    }

    public override bool CanCastSkill() {
        return base.CanCastSkill() && !this.IsInDamageShare;
    }

    protected override void Cast(Transform attackTarget){
        base.Cast(attackTarget);

        if (this.Type != TargetType.Hero) return ;
        if (this.IsInDamageShare) return ;
        StopAllCoroutines();
        StartCoroutine(ShareDamageAllHeroesCoroutine());
    }

    private IEnumerator ShareDamageAllHeroesCoroutine() {
        this.IsInDamageShare = true;
        List<Hero> heroes = new List<Hero>(BattleManager.Instance.HeroesInBattle);
        foreach (Hero h in heroes) {
            h.ShareDamageList(heroes);
        }

        yield return this.Timer;
        foreach (Hero h in heroes) {
            h.RemoveShareDamageList(heroes);
        }
        this.IsInDamageShare = false;
    }
}

