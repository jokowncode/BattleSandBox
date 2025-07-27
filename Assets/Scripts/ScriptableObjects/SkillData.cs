
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "PhantomSpirit/SkillData", fileName = "SkillData")]
public class SkillData : ScriptableObject{
    public string Name;
    public string Description;
    public TargetType TargetType;

    public float Cooldown;
    public float Distance;
    public float Force = 0.0f;
    
    // Skill Value -> ValueProperty * ValueMultiple
    public FighterProperty ValueProperty;
    public float ValueMultiple;
    
    public SkillDelivery SkillDeliveryPrefab;
}

