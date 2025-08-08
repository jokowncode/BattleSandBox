
using UnityEngine;

public class FlashSkillStart : SkillStart {

    public override void AdditionalProcedure(GameObject target){
        target.AddComponent<FlashPoint>();
    }

}

