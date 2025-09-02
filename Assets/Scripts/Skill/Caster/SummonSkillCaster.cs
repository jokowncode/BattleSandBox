
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SummonSkillCaster : SkillCaster{

    [SerializeField] private Fighter SummonPetPrefab;
    [SerializeField] private float HealthPercentage = 0.6f;
    [SerializeField] private float AttackPercentage = 0.6f;
    
    [Header("Formation")]
    [SerializeField] private float Angle = 30.0f; 
    [SerializeField] private float Radius = 2.0f;
    
    [HideInInspector] public int MaxSummonMeanwhileCount = 1;

    private const int MaxSummonCount = 5;
    private List<Fighter> SummonPets;

    protected override void Awake(){
        base.Awake();
        SummonPets = new List<Fighter>();
        this.MaxSummonMeanwhileCount = 1;
    }

    protected override void Cast(Transform _){
        for (int i = 0; i < this.MaxSummonMeanwhileCount; i++) {
            if (this.SummonPets.Count >= MaxSummonCount) return;
            SummonPet();
        }
    }

    private Vector3 GetSummonPetPosition(int index) {
        float angle = ((index - 1) * Angle) % 360f;
        return CalculatePosition(this.OwnedFighter.transform.position, angle, this.Radius);
    }
    
    private Vector3 CalculatePosition(Vector3 center, float angle, float radius) {
        float rad = angle * Mathf.Deg2Rad;
        float x = center.x + radius * Mathf.Cos(rad);
        float z = center.z + radius * Mathf.Sin(rad);
        return new Vector3(x, center.y, z);
    }

    private void SummonPet() {
        Fighter pet = Instantiate(SummonPetPrefab, GetSummonPetPosition(this.SummonPets.Count+1), Quaternion.identity);
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

    public void ClearPet() {
        if (this.SummonPets.Count != 0){
            foreach (Fighter pet in SummonPets){
                if(pet) Destroy(pet.gameObject);
            }
            this.SummonPets.Clear();
        }
    }

    public override bool CanCastSkill(){
        return base.CanCastSkill() && this.SummonPets.Count < MaxSummonCount;
    }
}

