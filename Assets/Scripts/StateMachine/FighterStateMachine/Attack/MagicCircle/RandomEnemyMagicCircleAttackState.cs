
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyMagicCircleAttackState : MagicCircleAttackState{

    [SerializeField] private int RandomEnemyCount = 3;

    protected override void Awake(){
        base.Awake();
        IsNeedTarget = false;
    }
    
    protected override void NormalAttack(){
        if(AttackParticle) AttackParticle.Play();
        List<Fighter> result = BattleManager.Instance.GetRandomCountFighter(Controller.AttackTargetType, this.RandomEnemyCount);
        foreach (Fighter target in result) {
            if (!target) continue;
            CastMagicCircle(target.transform, 1.0f / result.Count);
        }
    }
}

