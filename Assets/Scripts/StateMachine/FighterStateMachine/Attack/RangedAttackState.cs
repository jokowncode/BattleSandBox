
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
        
        // TODO: According To Current Dir -> Change AttackPos
        // float horizontalForward = Mathf.Sign(Controller.PetMove.RendererTransform.localScale.x);
        // attackPos += new Vector3(-horizontalForward, 0, 0);
        attackPos += new Vector3(1.0f, 0.0f, 0.0f);

        Vector3 targetPos = AttackTarget.Center.position;
        Vector3 attackVec = (targetPos - attackPos).normalized;
        Bullet bullet = Instantiate(BulletPrefab, attackPos, Quaternion.LookRotation(attackVec));
        bullet.SetDamageMessage(new EffectData{
            Value = Controller.PhysicsAttack + Controller.MagicAttack,
            Force = Controller.Force,
            TargetType = Controller.AttackTargetType
        });
        
        bullet.SetTargetPos(targetPos);
    }
}
