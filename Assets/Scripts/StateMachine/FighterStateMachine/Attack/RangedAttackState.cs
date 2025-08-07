
using UnityEngine;

public class RangedAttackState : AttackState {

    [SerializeField] private Bullet BulletPrefab;
    
    protected override void OnAttack(){
        if (IsNeedTarget && !this.AttackTarget) return;
        base.OnAttack();
        if(AttackParticle) AttackParticle.Play();
        
        Vector3 attackPos = Controller.AttackCaster.localPosition;
        float horizontalForward = Mathf.Sign(Controller.Move.RendererTransform.localScale.x);
        attackPos.x *= horizontalForward;
        attackPos = Controller.transform.TransformPoint(attackPos);

        Vector3 targetPos = AttackTarget.Center.position;
        Vector3 attackVec = (targetPos - attackPos).normalized;
        Bullet bullet = Instantiate(BulletPrefab, attackPos, Quaternion.LookRotation(attackVec));
        
        float critical = Random.value < Controller.Critical / 100.0f ? 1.5f : 1.0f;
        EffectData damageMsg = new EffectData{
            Value = (Controller.PhysicsAttack + Controller.MagicAttack) * critical,
            Force = Controller.Force,
            TargetType = Controller.AttackTargetType
        };
        bullet.SetDamageMessage(damageMsg);
        bullet.SetTarget(this.AttackTarget.Center);
#if DEBUG_MODE
        Debug.Log($"{this.gameObject.name} Attack(Ranged) : {damageMsg.Value}");
        Controller.TotalDamage += damageMsg.Value;
#endif
    }
}
