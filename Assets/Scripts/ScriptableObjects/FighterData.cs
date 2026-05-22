
using UnityEngine;

[CreateAssetMenu(menuName = "DeckBreakers/FighterData", fileName = "FighterData")]
public class FighterData : ScriptableObject{
    public string Name;
    [TextArea] public string Description;
    public int StarLevel;
    public float Shield;
    public float Health;
    public float PhysicsAttack;
    public float MagicAttack;
    public float Force;
    public float Speed;
    public float AttackRadius;
    public float Critical;
    public TargetType AttackTargetType;
    public FighterType Type;
    public Sprite WarehouseHeroPortrait;
    public Sprite DetailPortrait;
    public Sprite SkillPortrait;
    [HideInInspector] public float CooldownPercentage = 1.0f;
}

