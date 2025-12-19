
using UnityEngine;

public class RandomEnemyMagicCircleAttackState : MagicCircleAttackState{

    [SerializeField] private int RandomEnemyCount = 3;

    protected override void Awake(){
        base.Awake();
        IsNeedTarget = false;
    }
    
    protected override void NormalAttack(){
        if(AttackParticle) AttackParticle.Play();
        for (int i = 0; i < RandomEnemyCount; i++){
            Fighter fighter = BattleManager.Instance.GetRandomFighter(Controller.AttackTargetType);
            if (fighter) {
                CastMagicCircle(fighter.transform, 1.0f / RandomEnemyCount);
            }
        }
    }
}

