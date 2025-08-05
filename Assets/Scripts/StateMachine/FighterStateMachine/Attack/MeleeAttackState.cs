
using UnityEngine;
using UnityEngine.VFX;

public class MeleeAttackState : AttackState{
    
    protected override void OnAttack(){
        if (IsNeedTarget && !this.AttackTarget) return;
        base.OnAttack();
        if (AttackParticle) {
            Vector3 attackVec = AttackTarget.transform.position - transform.position;
            Vector3 XZ2XY = attackVec;
            XZ2XY.y = XZ2XY.z;
            XZ2XY.z = 0.0f;
            Vector3 attackPos = Controller.Center.position + XZ2XY.normalized;
            AttackParticle.transform.position = attackPos;

            float angleX = Vector3.SignedAngle(Vector3.forward, attackVec.normalized, Vector3.up);
            AttackParticle.transform.localRotation = Quaternion.Euler(angleX, 90.0f, 90.0f);
            AttackParticle.Play();
        }
        
        float critical = Random.value < Controller.Critical / 100.0f ? 1.5f : 1.0f;
        Controller.AttackTarget?.BeDamaged(new EffectData{
            Value = (Controller.PhysicsAttack + Controller.MagicAttack) * critical,
            Force = Controller.Force,
            TargetType = Controller.AttackTargetType
        });
    }
}
