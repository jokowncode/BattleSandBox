
using UnityEngine;

public class FurthestTargetSkillCaster : SkillCaster {
    protected override void Cast(Transform _) {
        Fighter fighter = BattleManager.Instance.FindFurthestTarget(OwnedFighter.transform.position);
        OwnedFighter.transform.position = fighter.transform.position + Vector3.right * OwnedFighter.AttackRadius;
        OwnedFighter.Move.ChangeForward(-1.0f);
        
        // TODO: Buff
        OwnedFighter.FighterPropertyChange(FighterProperty.Critical, PropertyModifyWay.Value, 50, true);
        OwnedFighter.FighterPropertyChange(FighterProperty.CooldownPercentage, PropertyModifyWay.Value, 50, true);
    }
}
