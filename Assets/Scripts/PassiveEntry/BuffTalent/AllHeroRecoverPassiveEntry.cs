
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllHeroRecoverPassiveEntry : PassiveEntry {

    [SerializeField] private BuffMiniData RecoverBuff;
    [SerializeField] private GameObject tickEffectPrefab;
    
    [SerializeField] private FighterType TargetFighterType;
    [SerializeField] private float RecoverMultiplier = 5.0f;

    private int TargetFighterCount;
    private Hero OwnedHero;
    
    public override void Construct(Hero ownedHero) {
        this.OwnedHero = ownedHero;
        foreach (Hero hero in BattleManager.Instance.HeroesInBattle) {
            if (hero.Type == TargetFighterType) {
                this.TargetFighterCount += 1;
            }
        }
        
        BattleManager.Instance.OnHeroEnterTheField += OnHeroEnterTheField;
        BattleManager.Instance.OnHeroExitTheField += OnHeroExitTheField;
        BattleManager.Instance.OnBattleStart += OnBattleStart;
    }

    private void OnBattleStart() {
        // 场上每存在一名牧师，所有单位每秒回复5点*N的生命值
        RecoverBuff.changedValue = this.TargetFighterCount * RecoverMultiplier;
        BuffData buffData = ScriptableObject.CreateInstance<BuffData>();
        buffData.longTimeEffectBuff = new List<BuffMiniData> { RecoverBuff };
        buffData.immediateEffectBuff = new List<BuffMiniData>();
        buffData.lastEffectBuff = new List<BuffMiniData>();
        buffData.duration = -1.0f;

        foreach(Hero hero in BattleManager.Instance.HeroesInBattle) {
            if (!hero.TryGetComponent(out Buff buff)) {
                buff = hero.AddComponent<Buff>();
            }
            if(tickEffectPrefab!=null)
                buff.tickEffectPrefab = tickEffectPrefab;
            buff.AddBuff(this.OwnedHero, hero, buffData);
        }
    }

    private void OnHeroExitTheField(Hero hero) {
        if (hero.Type == TargetFighterType) {
            this.TargetFighterCount -= 1;
        }
    }

    private void OnHeroEnterTheField(Hero hero) {
        if (hero.Type == TargetFighterType) {
            this.TargetFighterCount += 1;
        }
    }

    public override void Destruct(Hero hero) {
        BattleManager.Instance.OnHeroEnterTheField -= OnHeroEnterTheField;
        BattleManager.Instance.OnHeroExitTheField -= OnHeroExitTheField;
        BattleManager.Instance.OnBattleStart -= OnBattleStart;
    }
}

