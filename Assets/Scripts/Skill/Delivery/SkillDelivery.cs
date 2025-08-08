
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public abstract class SkillDelivery : MonoBehaviour {

    public EffectData EffectData{ get; private set; }
    
    protected Vector3 TargetPosition;
    protected SkillEffect Effect;

    public GameObject Caster{ get; private set; }

    public FighterType CasterType{ get; private set; }
    public List<SkillMiddle> SkillMiddlePlugins{ get; private set; }
    protected Vector3 MoveVec;

    protected virtual void Awake() {
        Effect = GetComponent<SkillEffect>();
    }

    protected void ApplyMiddlePlugin() {
        foreach (SkillMiddle middle in SkillMiddlePlugins) {
            //middle.AdditionalProcedure();
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

    public void SetPlugins(List<SkillMiddle> middlePlugins, List<SkillEnd> endPlugins, bool isNew){
        this.SkillMiddlePlugins = middlePlugins;
        if(isNew) this.SkillMiddlePlugins = new List<SkillMiddle>(middlePlugins);
        this.Effect.SetEndPlugins(endPlugins, isNew);
    }

    protected virtual bool TriggerCondition(Collider other) {
        return other.gameObject.layer == LayerMask.NameToLayer(this.EffectData.TargetType.ToString())
            || other.gameObject.layer == LayerMask.NameToLayer("Border");
    }
    
    protected abstract void TriggerTargetIn(Collider other);
    protected virtual void TriggerTargetOut(Collider other){ }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject != Caster && TriggerCondition(other)) {
            TriggerTargetIn(other);
        }   
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject != Caster && TriggerCondition(other)) {
            TriggerTargetOut(other);
        }   
    }

    private void OnDisable() {
        FireTrail trail = this.gameObject.GetComponentInChildren<FireTrail>();
        if (trail) {
            trail.transform.parent = null;
        }
    }
}
