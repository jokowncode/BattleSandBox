
using UnityEngine;

public  class FireTrailSkillStart : SkillStart{

    [SerializeField] private float spawnDistance = 1.0f;
    [SerializeField] private float DamagePercentage = 0.3f;
    [SerializeField] private FireTrailSingleVFX fireTrailPrefab;
    
    public override void AdditionalProcedure(GameObject target, float damage, Fighter owner, int count){
        FireTrail ft = target.AddComponent<FireTrail>();
        ft.spawnDistance = spawnDistance;
        ft.fireTrailPrefab = fireTrailPrefab;
        ft.SetDamage(damage * this.DamagePercentage);
        // GameObject trailInstance = Instantiate(fireTrailPrefab, target.transform.position, Quaternion.identity);
        // trailInstance.transform.SetParent(target.transform, worldPositionStays: true);
    }

}

