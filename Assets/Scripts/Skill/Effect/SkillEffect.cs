
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour {

    [SerializeField] protected ParticleSystem EffectParticle;

    public void ApplyEffect(Fighter influenceFighter, SkillEffectData skillEffect) {
        if(EffectParticle) EffectParticle.Play();
        Apply(influenceFighter, skillEffect);
    }

    protected abstract void Apply(Fighter influenceFighter, SkillEffectData skillEffect);
    
}

