
using UnityEngine;

public class SummonSkillCaster : SkillCaster{

    [SerializeField] private Fighter SummonPetPrefab;
    [SerializeField] private float HealthPercentage = 0.6f;
    [SerializeField] private float AttackPercentage = 0.6f;
    
    
    private Fighter SummonPet;
    
    protected override bool Cast(Transform _){
        if (this.SummonPet) return false;
        this.SummonPet = Instantiate(SummonPetPrefab, this.transform.position, Quaternion.identity);
        this.SummonPet.Health = OwnedFighter.Health * HealthPercentage;
        
        this.SummonPet.PhysicsAttack = SummonPet.Type == FighterType.Warrior ? OwnedFighter.MagicAttack * AttackPercentage : 0.0f;
        this.SummonPet.MagicAttack = SummonPet.Type != FighterType.Warrior ? OwnedFighter.MagicAttack * AttackPercentage : 0.0f;
        this.SummonPet.BattleStart();
        return true;
    }
}

