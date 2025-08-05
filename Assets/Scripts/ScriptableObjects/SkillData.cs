
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "PhantomSpirit/SkillData", fileName = "SkillData")]
public class SkillData : ScriptableObject{
    public string Name;
    public string Description;
    public Color UIColor;
    public TargetType TargetType;

    public float Cooldown;
    public float Distance;
    public float Force = 0.0f;
    public int MaxCastCount = -1;  // -1 is not limit

    public float Duration = 0.0f; // Magic Circle Duration
    
    // Skill Value -> ValueProperty * ValueMultiple
    public FighterProperty ValueProperty;
    public float ValueMultiple;

    public bool SkillNeedTarget = true;
    
    public SkillDelivery SkillDeliveryPrefab;

    public bool SkillNeedBuff = false;
    public BuffData BuffDataSO;
}

