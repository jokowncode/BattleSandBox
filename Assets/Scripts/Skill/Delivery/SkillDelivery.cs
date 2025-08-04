
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public abstract class SkillDelivery : MonoBehaviour {

    protected EffectData EffectData;
    protected Vector3 TargetPosition;
    protected SkillEffect Effect;

    private GameObject Caster;

    public FighterType CasterType{ get; private set; }
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

    public void StartDelivery(GameObject caster, Vector3 targetPos, EffectData effectData, FighterType casterType) {
        this.TargetPosition = targetPos;
        this.Caster = caster;
        this.CasterType = casterType;
        
        this.MoveVec = targetPos - this.transform.position;
        this.MoveVec.y = 0.0f;
        this.MoveVec.Normalize();
        this.EffectData = effectData;
    }

    public void SetPlugins(List<SkillMiddle> middlePlugins, List<SkillEnd> endPlugins){
        this.SkillMiddlePlugins = middlePlugins;
        this.Effect.SetEndPlugins(endPlugins);
    }

    protected virtual bool TriggerCondition(Collider other) {
        return other.gameObject.layer == LayerMask.NameToLayer(this.EffectData.TargetType.ToString())
            || other.gameObject.layer == LayerMask.NameToLayer("Border");
    }
    protected abstract void TriggerTarget(Collider other);

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject != Caster && TriggerCondition(other)) {
            TriggerTarget(other);
        }   
    }
}
