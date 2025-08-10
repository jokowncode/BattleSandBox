
using UnityEngine;

public class HealMinHealthPercentageState : AttackState{

    [SerializeField] private float HealPercentage = 0.3f;
    
    protected override void Awake(){
        base.Awake();
        IsNeedTarget = false;
    }

    protected override void OnAttack(){
        base.OnAttack();
        Fighter target = BattleManager.Instance.FindMinPercentagePropertyHero(FighterProperty.Health, Controller.AttackTargetType);
        if (!target) return;
        
        float critical = Random.value < Controller.Critical / 100.0f ? 1.5f : 1.0f;
        EffectData healMsg = new EffectData{
            TargetType = Controller.AttackTargetType,
            Force = 0.0f,
            Value = Controller.Health * HealPercentage * Controller.HealMultiplier * critical
        };
        target.BeHealed(healMsg);
#if DEBUG_MODE
        Debug.Log($"{this.gameObject.name} Heal : {healMsg.Value}");
        Controller.TotalDamage += healMsg.Value;
#endif
    }

    protected override void OnAttackEnd(){
        if (BattleManager.Instance.IsGameOver) {
            Controller.FighterIdle();
            return;
        }
        
        if (Controller.FighterSkillCaster && Controller.FighterSkillCaster.CanCastSkill()){
            Controller.ChangeState(FighterSkill);
        }
    }
}


