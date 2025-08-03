
using UnityEngine;

[CreateAssetMenu(menuName = "PhantomSpirit/FighterData", fileName = "FighterData")]
public class FighterData : ScriptableObject{
    public string Name;
    public float Health;
    public float Shield;
    public float PhysicsAttack;
    public float MagicAttack;
    public float Force;
    public float Speed;
    public float AttackRadius;
    public float Critical;
    public TargetType AttackTargetType;
    public FighterType Type;
}

