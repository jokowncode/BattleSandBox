
using UnityEngine;

public class HealMinHealthPercentageState : AttackState{

    [SerializeField] private float HealPercentage = 0.3f;
    
    protected override void Awake(){
        base.Awake();
        IsNeedTarget = false;
    }

    protected override void OnAttack(){
        Fighter target = BattleManager.Instance.FindMinPercentagePropertyHero(FighterProperty.Health);
        if (!target) return;
        target.BeHealed(new EffectData{
            TargetType = TargetType.Hero,
            Force = 0.0f,
            Value = Controller.Health * HealPercentage * Controller.HealMultiplier
        });
    }

    protected override void OnAttackEnd(){
        if (BattleManager.Instance.IsGameOver) {
            Controller.FighterAnimator.SetTrigger(AnimationParams.Idle);
            Controller.ChangeState(null);
            return;
        }
        
        if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill()){
            Controller.ChangeState(FighterSkill);
        }
    }
}


