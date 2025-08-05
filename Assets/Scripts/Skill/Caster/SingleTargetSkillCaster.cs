
using UnityEngine;

public class SingleTargetSkillCaster : SkillCaster {

    protected void InitializeSkillDelivery(Vector3 attackTargetPosition) {
        Vector3 selfPos = OwnedFighter.Center.transform.position;
        selfPos.y = attackTargetPosition.y;
        Vector3 moveVec = (attackTargetPosition - selfPos).normalized;
        SkillDelivery delivery = Instantiate(this.Data.SkillDeliveryPrefab, transform.position, Quaternion.LookRotation(moveVec));
        delivery.StartDelivery(this.gameObject, attackTargetPosition, new EffectData {
            TargetType = this.Data.TargetType,
            Force = this.Data.Force,
            Value = GetSkillEffectValue(),
            Duration = this.Data.Duration
        }, OwnedFighter.Type);
        delivery.SetPlugins(this.SkillMiddlePlugins, this.SkillEndPlugins);
    }

    protected override void Cast(Transform attackTarget){
        InitializeSkillDelivery(attackTarget.position);
    }
}


