
using UnityEngine;
using UnityEngine.VFX;

public class MeleeAttackState : AttackState{
    
    protected override void NormalAttack(){
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

        EffectData damageMsg = GetEffectData();
        Controller.AttackTarget?.BeDamaged(damageMsg);
#if DEBUG_MODE
        Debug.Log($"{this.gameObject.name} Attack(Melee) : {damageMsg.Value}");
        Controller.TotalDamage += damageMsg.Value;
#endif
    }
}
