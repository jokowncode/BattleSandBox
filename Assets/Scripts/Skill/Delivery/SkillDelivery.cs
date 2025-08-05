
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public abstract class SkillDelivery : MonoBehaviour {

    protected EffectData EffectData;
    protected Vector3 TargetPosition;
    protected SkillEffect Effect;
    
    public BuffData InitBuffData;
    private BuffData CurrentBuffData;

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

    public void InitializeBuffData(Fighter fighter)
    {
        CurrentBuffData = ScriptableObject.Instantiate(InitBuffData);
        
        CurrentBuffData.immediateEffectBuff.Clear();
        foreach(var miniData in InitBuffData.immediateEffectBuff)
            CurrentBuffData.immediateEffectBuff.Add(ScriptableObject.Instantiate(miniData));
        
        CurrentBuffData.longTimeEffectBuff.Clear();
        foreach(var miniData in InitBuffData.longTimeEffectBuff)
            CurrentBuffData.longTimeEffectBuff.Add(ScriptableObject.Instantiate(miniData));
        
        CurrentBuffData.lastEffectBuff.Clear();
        foreach(var miniData in InitBuffData.lastEffectBuff)
            CurrentBuffData.lastEffectBuff.Add(ScriptableObject.Instantiate(miniData));
        
        BuffUtils.InitializeBuffData(fighter,ref this.CurrentBuffData);
        Effect.CurrentBuffData = CurrentBuffData;
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

    public BuffData GetCurrentBuffData()
    {
        return CurrentBuffData;
    }
}
