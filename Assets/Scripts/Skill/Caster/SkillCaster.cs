
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillCaster : MonoBehaviour{

    [SerializeField] private SkillData InitialData;
    [SerializeField] private ParticleSystem SkillStartParticle;

    private Fighter OwnedFighter;

    private List<SkillStart> SkillStartPlugins;
    protected List<SkillMiddle> SkillMiddlePlugins;
    protected List<SkillEnd> SkillEndPlugins;

    private int CurrentSkillCastCount;
    private float LastCastTime;

    public SkillData Data{ get; private set; }

    public bool CanCastSkill => (Data.MaxCastCount <= 0 || CurrentSkillCastCount < Data.MaxCastCount)
                                && (!Data.SkillNeedTarget || OwnedFighter.AttackTarget != null)
                                && (LastCastTime < 0.0f || Time.time - LastCastTime > Data.Cooldown);

    private void Awake(){
        OwnedFighter = GetComponentInParent<Fighter>();
        SkillStartPlugins = new List<SkillStart>();
        SkillMiddlePlugins = new List<SkillMiddle>();
        SkillEndPlugins = new List<SkillEnd>();
        this.Data = Instantiate(InitialData);
    }

    public void BattleStart(){
        LastCastTime = -1.0f;
        if (OwnedFighter.Type != FighterType.Melee){
            LastCastTime = Time.time;
        }
    }

    #region SkillProcedurePlugin

    public void AddSkillStart(SkillStart skillStart){
        SkillStartPlugins.Add(skillStart);
    }

    public void AddSkillMiddle(SkillMiddle skillMiddle){
        SkillMiddlePlugins.Add(skillMiddle);
    }

    public void AddSkillEnd(SkillEnd skillEnd){
        SkillEndPlugins.Add(skillEnd);
    }

    public void RemoveSkillStart(SkillStart skillStart){
        SkillStartPlugins.Remove(skillStart);
    }

    public void RemoveSkillMiddle(SkillMiddle skillMiddle){
        SkillMiddlePlugins.Remove(skillMiddle);
    }

    public void RemoveSkillEnd(SkillEnd skillEnd){
        SkillEndPlugins.Remove(skillEnd);
    }

    #endregion

    protected float GetSkillEffectValue(){
        float property = ReflectionTools.GetObjectProperty<float>(Data.ValueProperty.ToString(), this.OwnedFighter);
        float percentage = Data.ValueMultiple / 100.0f;
        return property * percentage;
    }

    protected abstract void Cast(Transform attackTarget);

    public void CastSkill(Transform attackTarget){
        if (!CanCastSkill) return;
        // TODO: Play Skill Pre Anim
        if (SkillStartParticle) SkillStartParticle.Play();
        CurrentSkillCastCount++;
        Cast(attackTarget);
        foreach (SkillStart start in SkillStartPlugins){
            start.AdditionalProcedure();
        }

        // TODO: Play Skill After Anim
        LastCastTime = Time.time;
    }

    #region SkillPropretyChange

    private float GetInitialData(SkillProperty property){
        switch (property){
            case SkillProperty.Duration:
                return InitialData.Duration;
        }
        return -1.0f;
    }

    private float GetCurrentData(SkillProperty property) {
        switch (property){
            case SkillProperty.Duration:
                return Data.Duration;
        }
        return -1.0f;
    }

    private void SetData(SkillProperty property, float value) {
        switch (property){
            case SkillProperty.Duration:
                Data.Duration = value;
                break;
        }
    }

    public void SKillPropertyChange(SkillProperty property, PropertyModifyWay modifyWay, float value, bool isUp) {
        float currentValue = GetCurrentData(property);
        float sign = isUp ? 1.0f : -1.0f;
        
        switch (modifyWay){
            case PropertyModifyWay.Value:
                currentValue += sign * value;
                break;
            case PropertyModifyWay.Percentage:
                float percentage = value / 100.0f;
                float initialValue = GetInitialData(property);
                currentValue += sign * initialValue * percentage;
                break;
        }
        SetData(property, currentValue);
    }
    #endregion
}

