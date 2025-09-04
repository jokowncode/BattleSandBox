using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExecuteBuffSkillStart : SkillStart {
    
    public BuffData BuffData;
    
    public override void AdditionalProcedure(GameObject target, float damage,Fighter ownedFighter, int count) {
            BuffManager.Instance.AddBuff(ownedFighter, ownedFighter, BuffData);
    }
}
