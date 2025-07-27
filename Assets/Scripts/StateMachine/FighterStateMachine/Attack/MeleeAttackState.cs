
using UnityEngine;

public class MeleeAttackState : AttackState{
    
    protected override void Attack(){
        // TODO: Play Attack Anim
        if (AttackParticle) {
            // TODO: Play Attack Particle
            // NOTE: Transform XoZ To XoY
            /*Vector3 attackVec = AttackTarget.transform.position - transform.position;
            Vector3 XZ2XY = attackVec;
            XZ2XY.y = XZ2XY.z;
            XZ2XY.z = 0.0f;
            Vector3 attackPos = Controller.PetCenter.position + XZ2XY.normalized + Vector3.up;
            AttackParticle.transform.position = attackPos;
            
            float angleX = Vector3.SignedAngle(Vector3.forward, attackVec.normalized, Vector3.up);
            AttackParticle.transform.localRotation = Quaternion.Euler(angleX, 90.0f, 90.0f);
            AttackParticle.Play();*/
        }
        
        // TODO: Attack Target Be Attacked
        /*Controller.AttackTarget?.BeAttacked(new DamageMessage{
            Damage = Controller.Data.Damage,
            Force = Controller.Data.Force,
        });*/
    }
}
