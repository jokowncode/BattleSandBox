
using UnityEngine;

public class RangedAttackState : AttackState {

    [SerializeField] private PoolGO BulletPrefab;
    
    protected override void NormalAttack(){
        if(AttackParticle) AttackParticle.Play();
        
        Vector3 attackPos = Controller.AttackCaster.localPosition;
        float horizontalForward = Mathf.Sign(Controller.Move.RendererTransform.localScale.x);
        attackPos.x *= horizontalForward;
        attackPos = Controller.transform.TransformPoint(attackPos);

        Vector3 targetPos = AttackTarget.Center.position;
        Vector3 attackVec = (targetPos - attackPos).normalized;
        
        // Bullet bullet = Instantiate(BulletPrefab, attackPos, Quaternion.LookRotation(attackVec));

        PoolGO go = PoolManager.Instance.GetGameObject(this.BulletPrefab);
        if (!go.TryGetComponent(out Bullet bullet)) return;

        bullet.transform.position = attackPos;
        bullet.transform.rotation = Quaternion.LookRotation(attackVec);
        
        bool criticalTest = Random.value < Controller.Critical / 100.0f;
        float critical = criticalTest ? 1.5f : 1.0f;
        EffectData damageMsg = new EffectData{
            Value = (Controller.PhysicsAttack + Controller.MagicAttack) * critical,
            Force = Controller.Force,
            TargetType = Controller.AttackTargetType,
            IsCritical = criticalTest
        };
        bullet.SetDamageMessage(damageMsg);
        bullet.SetTarget(this.AttackTarget.Center);
#if DEBUG_MODE
        Debug.Log($"{this.gameObject.name} Attack(Ranged) : {damageMsg.Value}");
        Controller.TotalDamage += damageMsg.Value;
#endif
    }
}
