
using UnityEngine;

public abstract class BaseCircleSpecialAttack : MeleeAttackState {

    [Header("Special Attack")]
    [SerializeField] private Bullet BulletPrefab;
    [SerializeField] private float Angle = 60.0f;
    [SerializeField] private float InitialDistance = 2.0f;
    
    protected abstract override bool SpecialAttackCondition();

    protected override void SpecialAttack() {
        for (float a = 0.0f; a <= 360.0f; a += Angle) {
            Vector3 rotVec = (Quaternion.AngleAxis(a, this.transform.forward) * this.transform.right).normalized;
            Vector3 pos = this.Controller.AttackCaster.position + InitialDistance * rotVec;
            Bullet bullet = Instantiate(BulletPrefab, pos, Quaternion.LookRotation(rotVec));
            bool criticalTest = Random.value < Controller.Critical / 100.0f;
            float critical = criticalTest ? 1.5f : 1.0f;
            EffectData damageMsg = new EffectData{
                Value = (Controller.PhysicsAttack + Controller.MagicAttack) * critical,
                Force = Controller.Force,
                TargetType = Controller.AttackTargetType,
                IsCritical = criticalTest
            };
            bullet.SetDamageMessage(damageMsg);
            bullet.SetTargetDir(rotVec);
        }
    }
}

