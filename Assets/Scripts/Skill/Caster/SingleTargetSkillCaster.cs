
using UnityEngine;

public class SingleTargetSkillCaster : SkillCaster {
    
    protected override void Cast(Transform attackTarget) {
        Vector3 moveVec = (attackTarget.position - this.transform.position).normalized;
        SkillDelivery delivery = Instantiate(this.Data.SkillDeliveryPrefab, transform.position, Quaternion.LookRotation(moveVec));
        delivery.StartDelivery(this.gameObject, attackTarget.position, new EffectData {
            TargetType = this.Data.TargetType,
            Force = this.Data.Force,
            Value = GetSkillEffectValue()
        });
        delivery.SetPlugins(this.SkillMiddlePlugins, this.SkillEndPlugins);
    }
}


