
using UnityEngine;

public class HealState : AttackState{

    [SerializeField] private float HealPercentage = 0.3f;
    
    public override void Construct(){
        base.Construct();
        IsNeedTarget = false;
    }

    protected override void Attack(){
        Fighter target = BattleManager.Instance.FindMinPercentagePropertyHero(FighterProperty.Health);
        if (!target) return;
        target.BeHealed(new EffectData{
            TargetType = TargetType.Hero,
            Force = 0.0f,
            Value = Controller.Health * HealPercentage
        });
    }

    public override void Transition(){
        if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill){
            Controller.ChangeState(FighterSkill);
        }
    }
}


