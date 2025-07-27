
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour {

    [SerializeField] protected ParticleSystem EffectParticle;

    private List<SkillEnd> SkillEndPlugins;

    public void SetEndPlugins(List<SkillEnd> endPlugins) {
        this.SkillEndPlugins = endPlugins;
    }

    public void ApplyEffect(Fighter influenceFighter, EffectData effectData) {
        if(EffectParticle) EffectParticle.Play();
        Apply(influenceFighter, effectData);
        foreach (SkillEnd end in SkillEndPlugins) {
            end.AdditionalProcedure(influenceFighter, effectData);
        }
    }

    protected abstract void Apply(Fighter influenceFighter, EffectData effectData);
    
}

