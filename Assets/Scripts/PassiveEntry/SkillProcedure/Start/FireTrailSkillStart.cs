
using UnityEngine;

public  class FireTrailSkillStart : SkillStart {

    public GameObject fireTrailPrefab;
    
    public override void AdditionalProcedure(GameObject target)
    {
        Debug.Log("wow");
        // 实例化粒子特效预制体
        GameObject trailInstance = GameObject.Instantiate(fireTrailPrefab, target.transform.position, Quaternion.identity);

        // 将 trailInstance 设置为 target 的子对象，使其跟随移动
        trailInstance.transform.SetParent(target.transform, worldPositionStays: true);
        
    }

}

