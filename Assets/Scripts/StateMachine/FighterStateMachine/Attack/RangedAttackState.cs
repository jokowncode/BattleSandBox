
using System;
using UnityEngine;

public class RangedAttackState : AttackState {

    [SerializeField] private Bullet BulletPrefab;
    
    protected override void Attack(){
        // TODO: Play Attack Anim
        if(AttackParticle) AttackParticle.Play();
        
        // TODO: Cast Bullet
        // TODO: Has Up / Down ?
        Vector3 attackPos = Controller.Center.position;
        
        float horizontalForward = Mathf.Sign(Controller.Move.RendererTransform.localScale.x);
        attackPos += new Vector3(-horizontalForward, 0, 0);
        
        Vector3 targetPos = AttackTarget.Center.position;
        Vector3 attackVec = (targetPos - attackPos).normalized;
        Bullet bullet = Instantiate(BulletPrefab, attackPos, Quaternion.LookRotation(attackVec));
        bullet.SetDamageMessage(new EffectData{
            Value = Controller.PhysicsAttack + Controller.MagicAttack,
            Force = Controller.Force,
            TargetType = Controller.AttackTargetType
        });
        bullet.SetMoveVector(attackVec);
        // bullet.SetTargetPos(targetPos);
    }
}
