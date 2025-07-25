
using UnityEngine;

[CreateAssetMenu(menuName = "PhantomSpirit/SkillData", fileName = "SkillData")]
public class SkillData : ScriptableObject{
    public string Name;
    public string Description;
    public float Cooldown;
    public float Distance;
    
    public FighterProperty DamageProperty;
    public float DamageMultiple;
}

