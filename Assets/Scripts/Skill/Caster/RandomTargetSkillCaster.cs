
using UnityEngine;

public class RandomTargetSkillCaster : SingleTargetSkillCaster {
    protected override void Cast(Transform _){
        Fighter fighter = BattleFindCharacterTools.GetRandomFighter(this.Data.TargetType);
        if(fighter) base.Cast(fighter.transform);
    }
}

