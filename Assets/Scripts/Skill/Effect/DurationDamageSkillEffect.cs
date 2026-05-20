
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationDamageSkillEffect : SkillEffect{

    [Header("Duration Damage")]
    [SerializeField] private float DamageInterval = 1.0f;
    
    private List<Fighter> InMagicCircleAreaFighters;

    private float LastDamageTime = -1.0f;
    private EffectData DefaultEffectData;
    
    protected override void Awake(){
        base.Awake();
        this.InMagicCircleAreaFighters = new List<Fighter>();
    }

    public override void PrepareEffect() {
        this.InMagicCircleAreaFighters.Clear();
        this.LastDamageTime = -1.0f;
        if (this.Delivery) return;
        Fighter fighter = this.GetComponentInParent<Fighter>();
        float value = fighter.FighterSkillCaster.GetSkillEffectValue(out bool isCritical);
        this.DefaultEffectData = new EffectData {
            TargetType = fighter.FighterSkillCaster.Data.TargetType,
            Value = value,
            IsCritical = isCritical,
            NotShowParticle = false
        };
    }

    private void Update(){
        if (LastDamageTime > 0.0f && Time.time - LastDamageTime < DamageInterval) return;
        if (InMagicCircleAreaFighters.Count == 0) return;
        foreach (Fighter fighter in InMagicCircleAreaFighters) {
            EffectData data = this.Delivery ? this.Delivery.EffectData : this.DefaultEffectData;
            if(fighter) fighter.BeDamaged(data);
            #if DEBUG_MODE
                Debug.Log($"{this.Delivery.Caster.name} Cast Skill : {this.Delivery.EffectData.Value}");
            #endif 
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

    protected override void ApplySkillEndBuffToTarget(Fighter caster, Fighter _) {
        if (!this.SkillEndBuff) return;
        if (this.InMagicCircleAreaFighters.Count == 0) return;
        
        foreach (Fighter fighter in this.InMagicCircleAreaFighters) {
            BuffManager.Instance.AddBuff(caster, fighter, this.SkillEndBuff);
        }
    }

    protected override void ApplySkillEndPlugin(Fighter _, EffectData effectData) {
        if (this.SkillEndPlugins == null) return;
        if (this.InMagicCircleAreaFighters.Count == 0) return;
        
        // TODO: Need Optimize
        List<SkillEnd> occurSkillEnds = new();
        foreach (Fighter fighter in this.InMagicCircleAreaFighters) {
            occurSkillEnds.Clear();
            foreach (SkillEnd end in this.SkillEndPlugins) {
                if (occurSkillEnds.Contains(end)){
                    continue;
                }
                occurSkillEnds.Add(end);
                end.gameObject.SetActive(true);
                end.AdditionalProcedure(fighter, this, effectData);
            }
        }
        
        occurSkillEnds.Clear();
        for (int i = 0; i < this.SkillEndPlugins.Count; ) {
            SkillEnd end = this.SkillEndPlugins[i];
            if (occurSkillEnds.Contains(end)) {
                i++;
                continue;
            }
            this.SkillEndPlugins.Remove(end);
        }
    }
}
