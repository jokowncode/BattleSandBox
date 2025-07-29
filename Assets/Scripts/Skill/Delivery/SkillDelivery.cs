
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillDelivery : MonoBehaviour {

    protected EffectData EffectData;
    // private Vector3 TargetPosition;
    protected SkillEffect Effect;

    private GameObject Caster;
    
    public List<SkillMiddle> SkillMiddlePlugins{ get; private set; }
    protected Vector3 MoveVec;

    protected virtual void Awake() {
        Effect = GetComponent<SkillEffect>();
    }

    protected void ApplyMiddlePlugin() {
        foreach (SkillMiddle middle in SkillMiddlePlugins) {
            middle.AdditionalProcedure();
        }
    }

    public void StartDelivery(GameObject caster, Vector3 targetPos, EffectData effectData) {
        // this.TargetPosition = target;
        this.Caster = caster;
        this.MoveVec = (targetPos - this.transform.position).normalized;
        this.EffectData = effectData;
    }

    public void SetPlugins(List<SkillMiddle> middlePlugins, List<SkillEnd> endPlugins){
        this.SkillMiddlePlugins = middlePlugins;
        this.Effect.SetEndPlugins(endPlugins);
    }

    protected virtual bool CollisionCondition(Collision collision) {
        return collision.gameObject.layer == LayerMask.NameToLayer(this.EffectData.TargetType.ToString());
    }
    protected abstract void CollisionTarget(Collision collision);

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject != Caster && CollisionCondition(collision)) {
            CollisionTarget(collision);
        }   
    }
}
