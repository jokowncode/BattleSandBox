
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllHeroRecoverPassiveEntry : PassiveEntry {

    [SerializeField] private BuffMiniData RecoverBuff;
    [SerializeField] private FighterType TargetFighterType;
    [SerializeField] private float RecoverMultiplier = 5.0f;

    public override void Construct(Hero ownedHero) {
        BattleManager.Instance.OnBattleStart += OnBattleStart;
    }

    private void OnBattleStart() {
        // 场上每存在一名牧师，所有单位每秒回复5点*N的生命值
        BuffMiniData CurrentRecoverBuff = Instantiate(this.RecoverBuff);

        int count = 0;
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            if (hero.Type == TargetFighterType) {
                count += 1;
            }
        }
        
        CurrentRecoverBuff.ChangedValue = count * RecoverMultiplier;
        BuffData buffData = ScriptableObject.CreateInstance<BuffData>();
        buffData.LongTimeEffectBuff = new List<BuffMiniData> { CurrentRecoverBuff };
        buffData.ImmediateEffectBuff = new List<BuffMiniData>();
        buffData.LastEffectBuff = new List<BuffData>();
        buffData.Duration = -1.0f;

        foreach(Hero hero in BattleManager.Instance.HeroesInBattle) {
            BuffManager.Instance.AddBuff(hero, hero, buffData);
        }
    }

    public override void Destruct(Hero hero) {
        BattleManager.Instance.OnBattleStart -= OnBattleStart;
    }
}

