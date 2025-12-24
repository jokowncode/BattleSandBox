
using UnityEngine;

public abstract class BaseCircleSpecialAttack : MeleeAttackState {

    [Header("Special Attack")]
    [SerializeField] private PoolGO BulletPrefab;
    [SerializeField] private float Angle = 60.0f;
    [SerializeField] private float InitialDistance = 0.5f;
    
    protected abstract override bool SpecialAttackCondition();

    protected override void SpecialAttack() {
        for (float a = 0.0f; a <= 360.0f; a += Angle) {
            Vector3 rotVec = (Quaternion.AngleAxis(a, Vector3.up) * Vector3.right).normalized;
            Vector3 pos = this.Controller.Center.position + InitialDistance * rotVec;
            
            // Bullet bullet = Instantiate(BulletPrefab, pos, Quaternion.LookRotation(rotVec));

            PoolGO go = PoolManager.Instance.GetGameObject(this.BulletPrefab, null);
            if (!go.TryGetComponent(out Bullet bullet)) return;

            bullet.transform.position = pos;
            bullet.transform.rotation = Quaternion.LookRotation(rotVec);
            
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

