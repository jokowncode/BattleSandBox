using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class AssassinSkillStart : SkillStart{
    
    [SerializeField] private FireTrailSingleVFX fireTrailPrefab;
    
    
    public override void AdditionalProcedure(GameObject target, float damage, Fighter owner, int count){
        // FireTrail ft = target.AddComponent<FireTrail>();
        // ft.spawnDistance = spawnDistance;
        // ft.fireTrailPrefab = fireTrailPrefab;
        // ft.SetDamage(damage * this.DamagePercentage);
        // // GameObject trailInstance = Instantiate(fireTrailPrefab, target.transform.position, Quaternion.identity);
        // // trailInstance.transform.SetParent(target.transform, worldPositionStays: true);
    }
}
