
using UnityEngine;

public class HealMinHealthPercentageState : AttackState{

    [SerializeField] private float HealPercentage = 0.3f;
    
    protected override void Awake(){
        base.Awake();
        IsNeedTarget = false;
    }

    protected override void NormalAttack(){
        Fighter target = BattleManager.Instance.FindMinPercentagePropertyHero(FighterProperty.Health, Controller.AttackTargetType);
        if (!target) return;

        bool criticalTest = Random.value < Controller.Critical / 100.0f;
        float critical = criticalTest ? 1.5f : 1.0f;
        EffectData healMsg = new EffectData{
            TargetType = Controller.AttackTargetType,
            Force = 0.0f,
            Value = Controller.Health * HealPercentage * Controller.HealMultiplier * critical,
            IsCritical = criticalTest
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


