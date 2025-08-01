
using UnityEngine;

public class SingleTargetSkillCaster : SkillCaster {
    
    protected override void Cast(Transform attackTarget){
        Vector3 selfPos = OwnedFighter.Center.transform.position;
        selfPos.y = attackTarget.position.y;
        Vector3 moveVec = (attackTarget.position - selfPos).normalized;
        SkillDelivery delivery = Instantiate(this.Data.SkillDeliveryPrefab, transform.position, Quaternion.LookRotation(moveVec));
        delivery.StartDelivery(this.gameObject, attackTarget.position, new EffectData {
            TargetType = this.Data.TargetType,
            Force = this.Data.Force,
            Value = GetSkillEffectValue()
        });
        delivery.SetPlugins(this.SkillMiddlePlugins, this.SkillEndPlugins);
    }
}


