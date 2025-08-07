
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour {

    [SerializeField] protected ParticleSystem EffectParticle;
    [SerializeField] private AudioClip SkillApplyEffectSfx;

    public List<SkillEnd> SkillEndPlugins{ get; private set; }
    public SkillDelivery Delivery{ get; private set; }

    protected virtual void Awake(){
        Delivery = GetComponent<SkillDelivery>();
    }

    public void SetEndPlugins(List<SkillEnd> endPlugins, bool isNew){
        this.SkillEndPlugins = endPlugins;
        if(isNew) this.SkillEndPlugins = new List<SkillEnd>(endPlugins);
    }

    public void ApplyEffect(Fighter influenceFighter, EffectData effectData) {
        if (EffectParticle) {
            EffectParticle.transform.position = influenceFighter.transform.position + Vector3.up;
            EffectParticle.Play();
        }

        if (SkillApplyEffectSfx) {
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.SkillApplyEffectSfx);
        }

        Apply(influenceFighter, effectData);
        if (Delivery.CasterType == FighterType.Warrior) {
            CameraManager.Instance.ShakeCamera(0.5f, 0.5f, Vector3.up);
        } else {
            CameraManager.Instance.ShakeCamera(0.5f, 0.25f, Vector3.right);
        }
        
        Dictionary<SkillEnd, bool> occurSkillEnds = new Dictionary<SkillEnd, bool>();
        for (int i = 0; i < this.SkillEndPlugins.Count; ){
            SkillEnd end = this.SkillEndPlugins[i];
            if (!occurSkillEnds.TryAdd(end, true)){
                i += 1;
                continue;
            }
            this.SkillEndPlugins.Remove(end);
            end.AdditionalProcedure(influenceFighter, this, effectData);
        }
    }

    protected abstract void Apply(Fighter influenceFighter, EffectData effectData);
    
}

