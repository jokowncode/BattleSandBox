
using System;
using UnityEngine;

public class SkillCaster : MonoBehaviour{

    [SerializeField] private SkillData Data;
    [SerializeField] private ParticleSystem StartSkillParticle;
    [SerializeField] private SkillDelivery Delivery;

    private Animator FighterAnimator;
    
    private float LastCastTime;
    private bool CanCastSkill => Time.time - LastCastTime > Data.Cooldown;

    private void Awake(){
        FighterAnimator = GetComponent<Animator>();
    }

    public void CastSkill(){
        if (!CanCastSkill) return;
        // TODO: Play Skill Pre Anim
        if(StartSkillParticle) StartSkillParticle.Play();
        
        // TODO: Play Skill After Anim
    }
    
    
    
}

