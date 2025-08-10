
using UnityEngine;

public class AddBulletSkillStart : SkillStart{

    [SerializeField] private float Angle = 15.0f;
    
    public override void AdditionalProcedure(GameObject target, float damage, Fighter owner, int count){
        if (!target.TryGetComponent(out SkillDelivery delivery)) return;
        float sign = count % 2 == 0 ? -1.0f : 1.0f;
        int index = (count - 1) / 2 + 1;
        
        Quaternion rot = delivery.transform.rotation * Quaternion.AngleAxis(sign * index * Angle, Vector3.up); 
        SkillDelivery newBullet = Instantiate(delivery, delivery.transform.position, rot); 
        newBullet.StartDelivery(delivery.Caster, 
            newBullet.transform.position + newBullet.transform.forward,
            delivery.EffectData, delivery.CasterType);
        newBullet.SetPlugins(delivery.SkillMiddlePlugins, delivery.Effect.SkillEndPlugins, true);
    }
}

