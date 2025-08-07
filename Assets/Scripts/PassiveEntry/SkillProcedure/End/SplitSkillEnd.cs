
using System;
using UnityEngine;

// TODO: Max Split Times Check
public class SplitSkillEnd : SkillEnd{

    [SerializeField] private int SplitCount = 3;
    [SerializeField] private float BaseAngle = 30.0f;
    
    private void CastNewSkill(Fighter influenceFighter, SkillEffect effect, EffectData effectData, float angle){
        GameObject skill = Instantiate(effect.gameObject, effect.transform.position + Vector3.right, Quaternion.identity);
        if (!skill.TryGetComponent(out SkillDelivery delivery)) return;
        
        effectData.Value *= 1.0f / SplitCount;
        delivery.transform.rotation = effect.Delivery.transform.rotation * Quaternion.AngleAxis(angle, Vector3.up);
        
        delivery.StartDelivery(influenceFighter.gameObject, delivery.transform.position + delivery.transform.forward, effectData, delivery.CasterType);
        delivery.SetPlugins(effect.Delivery.SkillMiddlePlugins, effect.SkillEndPlugins, false);
    }

    public override void AdditionalProcedure(Fighter influenceFighter, SkillEffect effect, EffectData effectData){
        CastNewSkill(influenceFighter, effect, effectData, 0.0f);
        for (int i = 0; i < (SplitCount - 1) / 2; i++) {
            CastNewSkill(influenceFighter, effect, effectData, -(i + 1) * BaseAngle);
            CastNewSkill(influenceFighter, effect, effectData, (i + 1) * BaseAngle);
        }
    }
}

