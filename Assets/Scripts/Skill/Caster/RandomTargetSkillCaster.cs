
using UnityEngine;

public class RandomTargetSkillCaster : SingleTargetSkillCaster {
    protected override void Cast(Transform _){
        Fighter fighter = BattleManager.Instance.GetRandomFighter(this.Data.TargetType);
        if(fighter) InitializeSkillDelivery(fighter.transform.position);
    }
}

