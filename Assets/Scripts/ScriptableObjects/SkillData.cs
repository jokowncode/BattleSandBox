
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "DeckBreakers/SkillData", fileName = "SkillData")]
public class SkillData : ScriptableObject{
    public string Name;
    [TextArea] public string Description;
    public TargetType TargetType;

    public float Cooldown;
    public float Distance;
    public float Force = 0.0f;
    public bool CanCastAtStart = false; 
    
    public float Duration = 0.0f; // Magic Circle Duration
    
    // Skill Value -> ValueProperty * ValueMultiple
    public FighterProperty ValueProperty;
    public float ValueMultiple;

    public bool SkillNeedTarget = true;
    
    // public SkillDelivery SkillDeliveryPrefab;
    public PoolGO SkillDeliveryPrefab;
}

