using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllHeroBuffSkillStart : SkillStart {
    
    public BuffData BuffData;
    public GameObject immediateEffectPrefab;  // 立即效果粒子预制体
    public GameObject tickEffectPrefab;
    
    public override void AdditionalProcedure(GameObject target, float damage,Fighter ownedFighter, int count)
    {
        foreach(Fighter ft in BattleManager.Instance.HeroesInBattle)
        {
            if (!ft.TryGetComponent(out Buff buff))
            {
                buff = ft.AddComponent<Buff>();
            }

            if(immediateEffectPrefab!=null)
                buff.immediateEffectPrefab = immediateEffectPrefab;
            if(tickEffectPrefab!=null)
                buff.tickEffectPrefab = tickEffectPrefab;
            buff.AddBuff(ft, ft, BuffData);
        }
    }
}
