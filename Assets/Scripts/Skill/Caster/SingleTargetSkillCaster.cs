
using UnityEngine;

public class SingleTargetSkillCaster : SkillCaster {
    
    protected override void DeliverySkill(Transform attackTarget) {
        Vector3 moveVec = (attackTarget.position - this.transform.position).normalized;
        SkillDelivery delivery = Instantiate(this.SkillDeliveryPrefab, transform.position, Quaternion.LookRotation(moveVec));
        delivery.StartDelivery(attackTarget, new SkillEffectData {
            TargetType = this.Data.TargetType,
            Force = this.Data.Force,
            Value = GetDamage()
        });
    }
}


