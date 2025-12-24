
using UnityEngine;

public class SingleTargetSkillCaster : SkillCaster {

    protected void InitializeSkillDelivery(Vector3 attackTargetPosition) {
        Vector3 selfPos = OwnedFighter.Center.transform.position;
        selfPos.y = attackTargetPosition.y;
        Vector3 moveVec = (attackTargetPosition - selfPos).normalized;
        // SkillDelivery delivery = Instantiate(this.Data.SkillDeliveryPrefab, transform.position, Quaternion.LookRotation(moveVec));

        PoolGO go = PoolManager.Instance.GetGameObject(this.Data.SkillDeliveryPrefab, null);
        if (!go.TryGetComponent(out SkillDelivery delivery)) return;

        delivery.transform.position = this.transform.position;
        delivery.transform.rotation = Quaternion.LookRotation(moveVec);
        
        float value = GetSkillEffectValue(out bool isCritical);
        delivery.StartDelivery(this.OwnedFighter.gameObject, attackTargetPosition, new EffectData {
            TargetType = this.Data.TargetType,
            Force = this.Data.Force,
            Value = value,
            Duration = this.Data.Duration,
            IsCritical = isCritical
        }, OwnedFighter.Type);
        ApplySkillStart(delivery.gameObject, delivery.EffectData.Value);
        delivery.SetPlugins(this.SkillMiddlePlugins, this.SkillEndPlugins, true);
    }

    protected override void Cast(Transform attackTarget){
        InitializeSkillDelivery(attackTarget.position);
    }
}


