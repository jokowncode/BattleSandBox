
using System;
using UnityEngine;

public class SummonSkillCaster : SkillCaster{

    [SerializeField] private Fighter SummonPetPrefab;
    [SerializeField] private float HealthPercentage = 0.6f;
    [SerializeField] private float AttackPercentage = 0.6f;
    
    private Fighter SummonPet;
    
    protected override void Cast(Transform _){
        this.SummonPet = Instantiate(SummonPetPrefab, this.transform.position, Quaternion.identity);
        this.SummonPet.Health = OwnedFighter.Health * HealthPercentage;
        
        this.SummonPet.PhysicsAttack = SummonPet.Type == FighterType.Warrior ? OwnedFighter.MagicAttack * AttackPercentage : 0.0f;
        this.SummonPet.MagicAttack = SummonPet.Type != FighterType.Warrior ? OwnedFighter.MagicAttack * AttackPercentage : 0.0f;
        this.SummonPet.BattleStart();
        ApplySkillStart(this.SummonPet.gameObject, 
            SummonPet.Type == FighterType.Warrior ? this.SummonPet.PhysicsAttack : this.SummonPet.MagicAttack);
    }

    private void OnDestroy(){
        if(this.SummonPet) Destroy(this.SummonPet.gameObject);
    }

    public override bool CanCastSkill(){
        return base.CanCastSkill() && !this.SummonPet;
    }
}

