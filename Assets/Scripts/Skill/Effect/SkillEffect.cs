
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour {

    [SerializeField] protected ParticleSystem EffectParticle;

    public List<SkillEnd> SkillEndPlugins{ get; private set; }
    public SkillDelivery Delivery{ get; private set; }
    public BuffData CurrentBuffData{ get;set; }

    private void Awake(){
        Delivery = GetComponent<SkillDelivery>();
        CurrentBuffData = Delivery.GetCurrentBuffData();
    }

    public void SetEndPlugins(List<SkillEnd> endPlugins) {
        this.SkillEndPlugins = endPlugins;
    }

    public void ApplyEffect(Fighter influenceFighter, EffectData effectData) {
        if(EffectParticle) EffectParticle.Play();
        Apply(influenceFighter, effectData);
        foreach (SkillEnd end in SkillEndPlugins) {
            end.AdditionalProcedure(influenceFighter, this, effectData);
        }
    }

    protected abstract void Apply(Fighter influenceFighter, EffectData effectData);
    
}

