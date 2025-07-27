
using UnityEngine;

[CreateAssetMenu(menuName = "PhantomSpirit/FighterData", fileName = "FighterData")]
public class FighterData : ScriptableObject{
    public float Health;
    public float PhysicsAttack;
    public float MagicAttack;
    public float Force;
    public float Cooldown;
    public float AttackRadius;
    public float Critical;
    public float Speed = 15;
    public TargetType AttackTargetType;
}

