
using UnityEngine;

public class HealMinHealthPercentageState : AttackState{

    // [SerializeField] private float HealPercentage = 0.3f;
    
    protected override void Awake(){
        base.Awake();
        IsNeedTarget = false;
    }

    public override bool CanAttack() {
        return base.CanAttack() && 
               BattleFindCharacterTools.HasBeDamagedTarget(Controller.AttackTargetType);
    }

    protected override void NormalAttack(){
        Fighter target = BattleFindCharacterTools.FindMinHealthPercentageTarget(Controller.AttackTargetType);
        if (!target) return;
        Vector3 moveVec = target.Center.position - transform.position;
        Controller.Move.ChangeForward(moveVec.x);

        EffectData healMsg = GetEffectData();
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


