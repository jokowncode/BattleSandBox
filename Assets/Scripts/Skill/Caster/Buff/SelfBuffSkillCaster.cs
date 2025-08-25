
using UnityEngine;

public class SelfBuffSkillCaster : AddBuffSkillCaster {
    protected override void Cast(Transform attackTarget) {
        AddBuff(this.OwnedFighter);
    }
}

