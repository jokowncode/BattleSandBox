
using UnityEngine;

[CreateAssetMenu(menuName = "PhantomSpirit/FighterData", fileName = "FighterData")]
public class FighterData : ScriptableObject{
    public string Name;
    public string Description;
    public int StarLevel;
    public float Health;
    public float PhysicsAttack;
    public float MagicAttack;
    public float Force;
    public float Speed;
    public float AttackRadius;
    public float Critical;
    public TargetType AttackTargetType;
    public FighterType Type;
    public Sprite heroPortraitSprite;
    public Sprite standingSprite;
}

