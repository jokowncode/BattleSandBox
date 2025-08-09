
using UnityEngine;

public  class FireTrailSkillStart : SkillStart {

    public GameObject fireTrailPrefab;
    
    public override void AdditionalProcedure(SkillDelivery target){
        GameObject trailInstance = GameObject.Instantiate(fireTrailPrefab, target.transform.position, Quaternion.identity);
        trailInstance.transform.SetParent(target.transform, worldPositionStays: true);
    }

}

