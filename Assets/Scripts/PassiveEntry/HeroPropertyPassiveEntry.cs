
using UnityEngine;

public class HeroPropertyPassiveEntry : PassiveEntry{

    [SerializeField] private FighterProperty Property;
    [SerializeField] private PropertyModifyWay ModifyWay;
    [SerializeField] private float Value;
    
    
    
    public override void Construct(Hero hero){
        
    }

    public override void Destruct(Hero hero){
        
    }
    
}
