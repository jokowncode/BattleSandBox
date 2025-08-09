
using Unity.VisualScripting;
using UnityEngine;

public class FlashSkillStart : SkillStart{

    [SerializeField] private float DamagePercentage = 0.3f;

    public override void AdditionalProcedure(GameObject target, float damage,Fighter ownedFighter){
        FlashPoint point = target.AddComponent<FlashPoint>();
        point.SetDamage(damage * this.DamagePercentage);
    }

}

