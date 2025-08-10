
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SummonSkillCaster : SkillCaster{

    [SerializeField] private Fighter SummonPetPrefab;
    [SerializeField] private float HealthPercentage = 0.6f;
    [SerializeField] private float AttackPercentage = 0.6f;

    [HideInInspector] public int MaxSummonCount = 1;
    private List<Fighter> SummonPets;

    protected override void Awake(){
        base.Awake();
        SummonPets = new List<Fighter>();
        this.MaxSummonCount = 1;
    }

    protected override void Cast(Transform _){
        Fighter pet = Instantiate(SummonPetPrefab, this.transform.position, Quaternion.identity);
        pet.Health = OwnedFighter.Health * HealthPercentage;
        pet.Shield = pet.Health;
        
        pet.PhysicsAttack = pet.Type == FighterType.Warrior ? OwnedFighter.MagicAttack * AttackPercentage : 0.0f;
        pet.MagicAttack = pet.Type != FighterType.Warrior ? OwnedFighter.MagicAttack * AttackPercentage : 0.0f;
        pet.BattleStart();
        ApplySkillStart(pet.gameObject, 
            pet.Type == FighterType.Warrior ? pet.PhysicsAttack : pet.MagicAttack);

        pet.OnDead += () => this.SummonPets.Remove(pet);
        this.SummonPets.Add(pet);
    }

    private void OnDestroy(){
        if (this.SummonPets.Count != 0){
            foreach (Fighter pet in SummonPets){
                if(pet) Destroy(pet.gameObject);
            }
        }
    }

    public override bool CanCastSkill(){
        return base.CanCastSkill() && this.SummonPets.Count < this.MaxSummonCount;
    }
}

