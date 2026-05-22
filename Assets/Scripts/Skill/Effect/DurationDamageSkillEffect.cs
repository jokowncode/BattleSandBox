
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationDamageSkillEffect : SkillEffect{

    [Header("Duration Damage")]
    [SerializeField] private float DamageInterval = 1.0f;
    
    private List<Fighter> InMagicCircleAreaFighters;
    private HashSet<Fighter> AlreadyInfluenceFighters;

    private float LastDamageTime = -1.0f;
    private EffectData DefaultEffectData;

    private Fighter ParentFighter;
    
    protected override void Awake(){
        base.Awake();
        this.InMagicCircleAreaFighters = new List<Fighter>();
        this.AlreadyInfluenceFighters = new HashSet<Fighter>();
        this.IsAreaSkill = true;

        this.ParentFighter = this.GetComponentInParent<Fighter>();
    }

    public override void PrepareEffect() {
        this.InMagicCircleAreaFighters.Clear();
        this.LastDamageTime = -1.0f;
        if (this.Delivery || !this.ParentFighter || !this.ParentFighter.FighterSkillCaster) return;
        float value = this.ParentFighter.FighterSkillCaster.GetSkillEffectValue(out bool isCritical);
        this.DefaultEffectData = new EffectData {
            TargetType = this.ParentFighter.FighterSkillCaster.Data.TargetType,
            Value = value,
            IsCritical = isCritical,
            NotShowParticle = false
        };
    }

    private void Update(){
        if (LastDamageTime > 0.0f && Time.time - LastDamageTime < DamageInterval) return;
        if (InMagicCircleAreaFighters.Count == 0) return;
        EffectData data = this.Delivery ? this.Delivery.EffectData : this.DefaultEffectData;
        foreach (Fighter fighter in InMagicCircleAreaFighters) {
            if(fighter) fighter.BeDamaged(data);
            #if DEBUG_MODE
                Debug.Log($"{this.Delivery.Caster.name} Cast Skill : {this.Delivery.EffectData.Value}");
            #endif 
        }
        if (!this.BuffTargetIsSelf && this.Delivery && this.Delivery.TryGetComponent(out Fighter caster)) {
            this.ApplySkillEndBuffToTarget(caster);    
        }
        this.ApplySkillEndPlugin(data);
        foreach (Fighter fighter in this.InMagicCircleAreaFighters) {
            this.AlreadyInfluenceFighters.Add(fighter);
        }
        LastDamageTime = Time.time;
    }

    protected override void Apply(Fighter influenceFighter, EffectData effectData){ }

    private void OnTriggerEnter(Collider other) {
        if (!this.enabled) return;
        string layer = this.Delivery
            ? this.Delivery.EffectData.TargetType.ToString()
            : this.DefaultEffectData.TargetType.ToString();
        if (other.gameObject.layer == LayerMask.NameToLayer(layer)
            && other.gameObject.TryGetComponent(out Fighter fighter)){
            this.InMagicCircleAreaFighters.Add(fighter);
        }
    }

    private void OnTriggerExit(Collider other){
        if (!this.enabled) return;
        string layer = this.Delivery
            ? this.Delivery.EffectData.TargetType.ToString()
            : this.DefaultEffectData.TargetType.ToString();
        if (other.gameObject.layer == LayerMask.NameToLayer(layer)
            && other.gameObject.TryGetComponent(out Fighter fighter)) {
            this.InMagicCircleAreaFighters.Remove(fighter);
        }
    }

    private void ApplySkillEndBuffToTarget(Fighter caster) {
        if (!this.SkillEndBuff) return;
        if (this.InMagicCircleAreaFighters.Count == 0) return;
        
        foreach (Fighter fighter in this.InMagicCircleAreaFighters) {
            if(!fighter || this.AlreadyInfluenceFighters.Contains(fighter)) continue;
            BuffManager.Instance.AddBuff(caster, fighter, this.SkillEndBuff);
        }
    }

    private void ApplySkillEndPlugin(EffectData effectData) {
        if (this.SkillEndPlugins == null) return;
        if (this.InMagicCircleAreaFighters.Count == 0) return;
        
        HashSet<SkillEnd> uniqueSkillEnds = new HashSet<SkillEnd>(this.SkillEndPlugins);
        foreach (SkillEnd end in uniqueSkillEnds) {
            end.gameObject.SetActive(true);
            foreach (Fighter f in this.InMagicCircleAreaFighters) {
                if(!f || this.AlreadyInfluenceFighters.Contains(f)) continue;
                end.AdditionalProcedure(f, this, effectData);
            }
            this.SkillEndPlugins.Remove(end);
        }
    }
}
