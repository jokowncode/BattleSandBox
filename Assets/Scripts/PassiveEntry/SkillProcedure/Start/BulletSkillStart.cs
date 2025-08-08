
using UnityEngine;

public class BulletSkillStart : SkillStart {

    public override void AdditionalProcedure(GameObject target){
        target.AddComponent<FlashPoint>();
    }

}

