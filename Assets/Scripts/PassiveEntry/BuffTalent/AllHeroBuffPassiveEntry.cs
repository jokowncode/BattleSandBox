
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class AllHeroBuffPassiveEntry : PassiveEntry {

    [SerializeField] private BuffMiniData CastBuff;
    [SerializeField] private FighterType TargetFighterType;
    [SerializeField] private bool IsLongTimeBuff = true;
    [SerializeField] private float BuffDuration = -1.0f;
    [SerializeField] private float Multiplier = 5.0f;

    public override void Construct(Hero ownedHero) {
        BattleManager.Instance.OnBattleStart += OnBattleStart;
    }

    private void OnBattleStart() {
        // 场上每存在一名牧师，所有单位每秒回复5点*N的生命值
        BuffMiniData currentBuff = Instantiate(this.CastBuff);

        int count = 0;
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            if (hero.Type == TargetFighterType) {
                count += 1;
            }
        }
        
        currentBuff.ChangedValue = count * Multiplier;
        BuffData buffData = ScriptableObject.CreateInstance<BuffData>();
        
        if (this.IsLongTimeBuff) {
            buffData.LongTimeEffectBuff = new List<BuffMiniData> { currentBuff };
            buffData.ImmediateEffectBuff = new List<BuffMiniData>();
        } else {
            buffData.LongTimeEffectBuff = new List<BuffMiniData>();
            buffData.ImmediateEffectBuff = new List<BuffMiniData> {currentBuff};
        }
        
        buffData.LastEffectBuff = new List<BuffData>();
        buffData.Duration = this.BuffDuration;

        foreach(Hero hero in BattleManager.Instance.HeroesInBattle) {
            BuffManager.Instance.AddBuff(hero, hero, buffData);
        }
    }

    public override void Destruct(Hero hero) {
        BattleManager.Instance.OnBattleStart -= OnBattleStart;
    }
}

