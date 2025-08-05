
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour {

    [SerializeField] protected ParticleSystem EffectParticle;

    public List<SkillEnd> SkillEndPlugins{ get; private set; }
    public SkillDelivery Delivery{ get; private set; }

    protected virtual void Awake(){
        Delivery = GetComponent<SkillDelivery>();
    }

    public void SetEndPlugins(List<SkillEnd> endPlugins) {
        this.SkillEndPlugins = endPlugins;
    }

    public void ApplyEffect(Fighter influenceFighter, EffectData effectData) {
        if (EffectParticle) {
            EffectParticle.transform.position = influenceFighter.transform.position + Vector3.up;
            EffectParticle.Play();
        }

        Apply(influenceFighter, effectData);
        if (Delivery.CasterType == FighterType.Warrior) {
            CameraManager.Instance.ShakeCamera(0.5f, 0.5f, Vector3.up);
        } else {
            CameraManager.Instance.ShakeCamera(0.5f, 0.25f, Vector3.right);
        }
        foreach (SkillEnd end in SkillEndPlugins) {
            end.AdditionalProcedure(influenceFighter, this, effectData);
        }
    }

    protected abstract void Apply(Fighter influenceFighter, EffectData effectData);
    
}

