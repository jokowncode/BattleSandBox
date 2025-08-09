
using UnityEngine;

public class SkillPropertyPassiveEntry : PassiveEntry{

    [SerializeField] private SkillProperty Property;
    [SerializeField] private PropertyModifyWay ModifyWay;
    [SerializeField] private float Value;
    
    private void OnValidate(){
        if (ModifyWay == PropertyModifyWay.Percentage){
            Value = Mathf.Clamp(Value, -100.0f, 100.0f);
        }

        if (Property == SkillProperty.SummonCount){
            ModifyWay = PropertyModifyWay.Value;
        }
    }
    
    public override void Construct(Hero hero){
        hero.FighterSkillCaster.SKillPropertyChange(this.Property, this.ModifyWay, this.Value, true);
    }

    public override void Destruct(Hero hero){
        hero.FighterSkillCaster.SKillPropertyChange(this.Property, this.ModifyWay, this.Value, false);
    }
}

