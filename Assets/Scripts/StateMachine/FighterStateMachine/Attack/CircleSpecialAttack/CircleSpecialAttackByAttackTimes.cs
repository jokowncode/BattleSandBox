using UnityEngine;

public class CircleSpecialAttackByAttackTimes : BaseCircleSpecialAttack {

    [Header("Trigger")]
    [SerializeField] private int TriggerSpecialAttackInterval = 4;
    
    private int AttackTimes;

    protected override void NormalAttack() {
        base.NormalAttack();
        this.AttackTimes += 1;
    }
    
    protected override bool SpecialAttackCondition() {
        return (this.AttackTimes + 1) % this.TriggerSpecialAttackInterval == 0;
    }
}