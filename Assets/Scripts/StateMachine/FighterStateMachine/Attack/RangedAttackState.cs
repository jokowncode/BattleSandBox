
using System;
using UnityEngine;

public class RangedAttackState : AttackState {

    [SerializeField] private Bullet BulletPrefab;
    
    protected override void Attack(){
        if(AttackParticle) AttackParticle.Play();
        
        Vector3 attackPos = Controller.AttackCaster.position;
        float horizontalForward = Mathf.Sign(Controller.Move.RendererTransform.localScale.x);
        attackPos.x *= horizontalForward;

        Vector3 targetPos = AttackTarget.Center.position;
        Vector3 attackVec = (targetPos - attackPos).normalized;
        Bullet bullet = Instantiate(BulletPrefab, attackPos, Quaternion.LookRotation(attackVec));
        bullet.SetDamageMessage(new EffectData{
            Value = Controller.PhysicsAttack + Controller.MagicAttack,
            Force = Controller.Force,
            TargetType = Controller.AttackTargetType
        });
        bullet.SetMoveVector(attackVec);
    }
}
