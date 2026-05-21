
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour {

    [SerializeField] private AudioClip SkillApplyEffectSfx;

    [Header("Skill End Buff")]
    [SerializeField] protected bool BuffTargetIsSelf = true;
    [field: SerializeField] public BuffData SkillEndBuff { get; private set; }
    
    public List<SkillEnd> SkillEndPlugins{ get; private set; }
    public SkillDelivery Delivery{ get; private set; }
    public PoolGO InPoolGO { get; private set; }

    protected bool IsAreaSkill = false;

    protected virtual void Awake(){
        Delivery = GetComponent<SkillDelivery>();
        InPoolGO = GetComponent<PoolGO>();
    }

    public virtual void PrepareEffect() { }

    public void SetEndPlugins(List<SkillEnd> endPlugins, bool isNew){
        this.SkillEndPlugins = endPlugins;
        if(isNew && endPlugins != null) this.SkillEndPlugins = new List<SkillEnd>(endPlugins);
    }

    public void ApplyEffect(Fighter influenceFighter, EffectData effectData) {
        if (SkillApplyEffectSfx) {
            AudioManager.Instance.PlaySfx(this.SkillApplyEffectSfx);
        }

        Apply(influenceFighter, effectData);
        if (Delivery.CasterType == FighterType.Warrior) {
            CameraManager.Instance.ShakeCamera(0.5f, 0.5f, Vector3.up);
        } else {
            CameraManager.Instance.ShakeCamera(0.5f, 0.25f, Vector3.right);
        }

        if (SkillEndBuff && this.Delivery.Caster.TryGetComponent(out Fighter caster)) {
            if (this.BuffTargetIsSelf) BuffManager.Instance.AddBuff(caster, caster, SkillEndBuff);
            else if(!this.IsAreaSkill) BuffManager.Instance.AddBuff(caster, influenceFighter, SkillEndBuff);
        }
        if (!this.IsAreaSkill) {
            if (this.SkillEndPlugins == null) return;
            HashSet<SkillEnd> uniqueSkillEnds = new HashSet<SkillEnd>(this.SkillEndPlugins);
            foreach (SkillEnd end in uniqueSkillEnds) {
                end.gameObject.SetActive(true);
                end.AdditionalProcedure(influenceFighter, this, effectData);
                this.SkillEndPlugins.Remove(end);
            }
        }
    }

    protected abstract void Apply(Fighter influenceFighter, EffectData effectData);
    
}

