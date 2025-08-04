
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
        hero.FighterPropertyChange(this.Property, this.ModifyWay, this.Value, true);
    }

    public override void Destruct(Hero hero){
        hero.FighterPropertyChange(this.Property, this.ModifyWay, this.Value, false);
    }
    
}
