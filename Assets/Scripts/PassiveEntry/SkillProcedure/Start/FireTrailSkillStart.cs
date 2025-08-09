
using UnityEngine;

public  class FireTrailSkillStart : SkillStart {

    public GameObject fireTrailPrefab;
    
    public override void AdditionalProcedure(GameObject target, float damage){
        GameObject trailInstance = Instantiate(fireTrailPrefab, target.transform.position, Quaternion.identity);
        trailInstance.transform.SetParent(target.transform, worldPositionStays: true);
    }

}

