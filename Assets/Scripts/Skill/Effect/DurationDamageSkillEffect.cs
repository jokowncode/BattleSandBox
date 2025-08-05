
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationDamageSkillEffect : SkillEffect{

    [SerializeField] private float DamageInterval = 1.0f;

    private EffectData DamageMessage;
    private List<Fighter> InMagicCircleAreaFighters;
    private WaitForSeconds IntervalTimer;
    
    protected override void Awake(){
        base.Awake();
        this.InMagicCircleAreaFighters = new List<Fighter>();
        this.IntervalTimer = new WaitForSeconds(this.DamageInterval);
    }

    private void Start(){
        StartCoroutine(MagicCircleDamageCoroutine());
    }
    
    private IEnumerator MagicCircleDamageCoroutine(){
        if (InMagicCircleAreaFighters.Count == 0) yield return this.IntervalTimer;
        foreach (Fighter fighter in InMagicCircleAreaFighters){
            fighter.BeDamaged(this.DamageMessage);
        }
        yield return this.IntervalTimer;
    }

    protected override void Apply(Fighter influenceFighter, EffectData effectData){
        this.DamageMessage = effectData;
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.layer == LayerMask.NameToLayer(this.DamageMessage.TargetType.ToString())
            && other.gameObject.TryGetComponent(out Fighter fighter)) {
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
