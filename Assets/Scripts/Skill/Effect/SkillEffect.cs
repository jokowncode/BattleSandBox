
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour {

    [SerializeField] private AudioClip SkillApplyEffectSfx;

    [Header("Skill End Buff")]
    [SerializeField] private bool BuffTargetIsSelf = true;
    [field: SerializeField] public BuffData SkillEndBuff { get; private set; }
    
    public List<SkillEnd> SkillEndPlugins{ get; private set; }
    public SkillDelivery Delivery{ get; private set; }
    public PoolGO InPoolGO { get; private set; }

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
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.SkillApplyEffectSfx);
        }

        Apply(influenceFighter, effectData);
        if (Delivery.CasterType == FighterType.Warrior) {
            CameraManager.Instance.ShakeCamera(0.5f, 0.5f, Vector3.up);
        } else {
            CameraManager.Instance.ShakeCamera(0.5f, 0.25f, Vector3.right);
        }

        if (SkillEndBuff && this.Delivery.Caster.TryGetComponent(out Fighter caster)) {
            if (this.BuffTargetIsSelf) BuffManager.Instance.AddBuff(caster, caster, SkillEndBuff);
            else this.ApplySkillEndBuffToTarget(caster, influenceFighter);
        }
        this.ApplySkillEndPlugin(influenceFighter, effectData);
    }

    protected virtual void ApplySkillEndBuffToTarget(Fighter caster, Fighter target) {
        BuffManager.Instance.AddBuff(caster, target, SkillEndBuff);
    }

    protected virtual void ApplySkillEndPlugin(Fighter influenceFighter, EffectData effectData) {
        if (this.SkillEndPlugins == null) return;
        Dictionary<SkillEnd, bool> occurSkillEnds = new Dictionary<SkillEnd, bool>();
        for (int i = 0; i < this.SkillEndPlugins.Count; ){
            SkillEnd end = this.SkillEndPlugins[i];
            if (!occurSkillEnds.TryAdd(end, true)){
                i += 1;
                continue;
            }
            this.SkillEndPlugins.Remove(end);
            end.gameObject.SetActive(true);
            end.AdditionalProcedure(influenceFighter, this, effectData);
        }
    }

    protected abstract void Apply(Fighter influenceFighter, EffectData effectData);
    
}

