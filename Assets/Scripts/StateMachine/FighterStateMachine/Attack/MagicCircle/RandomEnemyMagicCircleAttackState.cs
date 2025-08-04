
using UnityEngine;

public class RandomEnemyMagicCircleAttackState : MagicCircleAttackState{

    [SerializeField] private int RandomEnemyCount = 3;

    protected override void OnAttack(){
        if(AttackParticle) AttackParticle.Play();
        for (int i = 0; i < RandomEnemyCount; i++){
            Fighter fighter = BattleManager.Instance.GetRandomFighter(Controller.AttackTargetType);
            if (fighter) {
                CastMagicCircle(fighter.transform.position, 1.0f / RandomEnemyCount);
            }
        }
    }
}

