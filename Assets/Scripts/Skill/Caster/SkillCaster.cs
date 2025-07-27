
using System;
using UnityEngine;

public abstract class SkillCaster : MonoBehaviour{

    [SerializeField] protected SkillData Data;
    [SerializeField] private ParticleSystem StartSkillParticle;
    [SerializeField] protected SkillDelivery SkillDeliveryPrefab;
    
    private Fighter OwnedFighter;
    
    private float LastCastTime;
    private bool CanCastSkill => Time.time - LastCastTime > Data.Cooldown;

    private void Awake(){
        OwnedFighter = GetComponentInParent<Fighter>();
    }

    protected float GetDamage() {
        float property = ReflectionTools.GetObjectProperty<float>(Data.ValueProperty.ToString(), this.OwnedFighter);
        float percentage = Data.ValueMultiple / 100.0f;
        return property * percentage;
    }

    protected abstract void DeliverySkill(Transform attackTarget);

    public void CastSkill(Transform attackTarget){
        if (!CanCastSkill) return;
        // TODO: Play Skill Pre Anim
        if(StartSkillParticle) StartSkillParticle.Play();
        DeliverySkill(attackTarget);
        // TODO: Play Skill After Anim
        LastCastTime = Time.time;
    }
}

