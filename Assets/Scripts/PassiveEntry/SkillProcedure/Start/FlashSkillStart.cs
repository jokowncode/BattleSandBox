
using Unity.VisualScripting;
using UnityEngine;

public class FlashSkillStart : SkillStart{

    [SerializeField] private float DamagePercentage = 0.3f;

    public override void AdditionalProcedure(SkillDelivery target){
        FlashPoint point = target.AddComponent<FlashPoint>();
        point.SetDamage(target.EffectData.Value * this.DamagePercentage);
    }

}

