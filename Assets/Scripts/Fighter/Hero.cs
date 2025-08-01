using System;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Fighter {
    
    private List<PassiveEntry> PassiveEntries;

    protected override void Awake(){
        base.Awake();
        PassiveEntries = new List<PassiveEntry>();
    }

    public void AddPassiveEntry(PassiveEntry entry){
        PassiveEntries.Add(entry);
        entry.Construct(this);
    }

    public void RemovePassiveEntry(int index){
        // TODO: Index Out of Range
        PassiveEntry entry = PassiveEntries[index];
        entry.Destruct(this);
        PassiveEntries.RemoveAt(index);
    }

    public void HeroPropertyChange(FighterProperty property, PropertyModifyWay modifyWay, float value, bool isUp){

        float sign = isUp ? 1.0f : -1.0f;
        if (property == FighterProperty.HealMultiplier) {
            float percentage = value / 100.0f;
            this.HealMultiplier += sign * percentage;
            return;
        }
        
        if (property == FighterProperty.ShieldMultiplier) {
            float percentage = value / 100.0f;
            this.ShieldMultiplier += sign * percentage;
            return;
        }
        
        if (property == FighterProperty.CooldownPercentage){
            float currentMultiplier = FighterAnimator.GetFloat(AnimationParams.AttackAnimSpeedMultiplier);
            float percentage = value / 100.0f;
            FighterAnimator.SetFloat(AnimationParams.AttackAnimSpeedMultiplier, currentMultiplier + sign * percentage);
            return;
        }

        string propertyName = property.ToString();
        float currentValue = ReflectionTools.GetObjectProperty<float>(propertyName, this);
        switch (modifyWay){
            case PropertyModifyWay.Value:
                currentValue += sign * value;
                break;
            case PropertyModifyWay.Percentage:
                float percentage = value / 100.0f;
                float initialValue = ReflectionTools.GetObjectProperty<float>("Initial"+propertyName, this);
                currentValue += sign * initialValue * percentage;
                break;
        }
        ReflectionTools.SetObjectProperty(propertyName, this, currentValue);
    }
}
