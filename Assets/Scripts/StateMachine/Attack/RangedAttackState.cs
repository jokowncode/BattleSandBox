
using System;
using UnityEngine;

public class RangedAttackState : AttackState {

    // [SerializeField] private Bullet BulletPrefab;
    
    protected override void Attack(){
        // TODO: Play Attack Anim
        if(AttackParticle) AttackParticle.Play();
        
        // TODO: Cast Bullet
        // TODO: Has Up / Down ?
        /*Vector3 attackPos = Controller.PetCenter.position;
        float horizontalForward = Mathf.Sign(Controller.PetMove.RendererTransform.localScale.x);
        attackPos += new Vector3(-horizontalForward, 0, 0);

        Vector3 targetPos = AttackTarget.Aim.position;
        Vector3 attackVec = (targetPos - attackPos).normalized;
        Bullet bullet = Instantiate(BulletPrefab, attackPos, Quaternion.LookRotation(attackVec));
        bullet.SetDamageMessage(new DamageMessage{
            Damage = Controller.Data.Damage,
            Force = Controller.Data.Force,
        });
        
        bullet.SetTargetPos(targetPos);*/
    }
}
