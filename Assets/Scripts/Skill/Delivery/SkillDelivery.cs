
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillDelivery : MonoBehaviour {

    protected EffectData EffectData;
    protected Transform Target;
    protected SkillEffect Effect;
    
    private List<SkillMiddle> SkillMiddlePlugins;

    protected Vector3 MoveVec => (Target.position - this.transform.position).normalized;
    
    protected virtual void Awake() {
        Effect = GetComponent<SkillEffect>();
    }

    protected void ApplyMiddlePlugin() {
        foreach (SkillMiddle middle in SkillMiddlePlugins) {
            middle.AdditionalProcedure();
        }
    }

    public void StartDelivery(Transform target, EffectData effectData, 
        List<SkillMiddle> middlePlugins, List<SkillEnd> endPlugins) {
        this.Target = target;
        this.EffectData = effectData;
        this.SkillMiddlePlugins = middlePlugins;
        this.Effect.SetEndPlugins(endPlugins);
    }

    protected virtual bool CollisionCondition(Collision collision) {
        return collision.gameObject.layer == LayerMask.NameToLayer(this.EffectData.TargetType.ToString());
    }
    protected abstract void CollisionTarget(Collision collision);

    private void OnCollisionEnter(Collision collision) {
        if (CollisionCondition(collision)) {
            CollisionTarget(collision);
        }   
    }
}
