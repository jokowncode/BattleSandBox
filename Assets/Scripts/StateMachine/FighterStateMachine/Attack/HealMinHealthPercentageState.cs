
using UnityEngine;

public class HealMinHealthPercentageState : AttackState{

    [SerializeField] private float HealPercentage = 0.3f;

    private PatrolState FighterPatrol;
    
    protected override void Awake(){
        base.Awake();
        IsNeedTarget = false;
        this.FighterPatrol = this.GetComponent<PatrolState>();
    }

    public override bool CanAttack() {
        return base.CanAttack() && 
               BattleManager.Instance.HasBeDamagedTarget(Controller.AttackTargetType);
    }

    protected override void NormalAttack(){
        Fighter target = BattleManager.Instance.FindMinPercentagePropertyHero(FighterProperty.Health, Controller.AttackTargetType);
        if (!target) return;
        Vector3 moveVec = target.Center.position - transform.position;
        Controller.Move.ChangeForward(moveVec.x);

        bool criticalTest = Random.value < Controller.Critical / 100.0f;
        float critical = criticalTest ? 1.5f : 1.0f;
        EffectData healMsg = new EffectData{
            TargetType = Controller.AttackTargetType,
            Force = 0.0f,
            Value = Controller.Health * HealPercentage * critical,
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
            return;
        }

        if (!this.CanAttack()) {
            Controller.ChangeState(this.FighterPatrol);
        }
    }
}


