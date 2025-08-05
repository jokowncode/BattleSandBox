
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationDamageSkillEffect : SkillEffect{

    [SerializeField] private float DamageInterval = 1.0f;

    private EffectData DamageMessage;
    private List<Fighter> InMagicCircleAreaFighters;

    private float LastDamageTime = -1.0f;
    
    protected override void Awake(){
        base.Awake();
        this.InMagicCircleAreaFighters = new List<Fighter>();
    }

    private void Update(){
        if (LastDamageTime > 0.0f && Time.time - LastDamageTime < DamageInterval) return;
        if (InMagicCircleAreaFighters.Count == 0) return;
        foreach (Fighter fighter in InMagicCircleAreaFighters){
            fighter.BeDamaged(this.DamageMessage);
        }
        LastDamageTime = Time.time;
    }

    protected override void Apply(Fighter influenceFighter, EffectData effectData){
        this.DamageMessage = effectData;
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.layer == LayerMask.NameToLayer(this.DamageMessage.TargetType.ToString())
            && other.gameObject.TryGetComponent(out Fighter fighter)){
            this.InMagicCircleAreaFighters.Add(fighter);
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject.layer == LayerMask.NameToLayer(this.DamageMessage.TargetType.ToString())
            && other.gameObject.TryGetComponent(out Fighter fighter)) {
            this.InMagicCircleAreaFighters.Remove(fighter);
        }
    }
}
