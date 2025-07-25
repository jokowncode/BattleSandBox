
using System;
using UnityEngine;

public class HeroPropertyPassiveEntry : PassiveEntry{

    [SerializeField] private FighterProperty Property;
    [SerializeField] private PropertyModifyWay ModifyWay;
    [SerializeField] private float Value;

    private void OnValidate(){
        if (ModifyWay == PropertyModifyWay.Percentage){
            Value = Mathf.Clamp(Value, -100.0f, 100.0f);
        }
    }

    public override void Construct(Hero hero){
        string propertyName = Property.ToString();
        float currentValue = ReflectionTools.GetObjectProperty<float>(propertyName, hero);
        
        switch (ModifyWay){
            case PropertyModifyWay.Value:
                currentValue += Value;
                break;
            case PropertyModifyWay.Percentage:
                float percentage = Value / 100.0f;
                float initialValue = ReflectionTools.GetObjectProperty<float>("Initial"+propertyName, hero);
                currentValue += initialValue * percentage;
                break;
        }
        ReflectionTools.SetObjectProperty(propertyName, hero, currentValue);
    }

    public override void Destruct(Hero hero){
        string propertyName = Property.ToString();
        float currentValue = ReflectionTools.GetObjectProperty<float>(propertyName, hero);

        switch (ModifyWay){
            case PropertyModifyWay.Value:
                currentValue -= Value;
                break;
            case PropertyModifyWay.Percentage:
                float percentage = Value / 100.0f;
                float initialValue = ReflectionTools.GetObjectProperty<float>("Initial"+propertyName, hero);
                currentValue -= initialValue * percentage;
                break;
        }
        ReflectionTools.SetObjectProperty(propertyName, hero, currentValue);
    }
    
}
