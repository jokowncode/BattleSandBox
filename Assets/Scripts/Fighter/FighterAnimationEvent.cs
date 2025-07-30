
using System;
using UnityEngine;

public class FighterAnimationEvent : MonoBehaviour{

    public Action OnAttack;
    public Action OnSkill;
    public Action OnSkillEnd;
    public Action OnAttackEnd;
    
    public void Attack(){
        OnAttack?.Invoke();
    }

    public void AttackEnd() {
        OnAttackEnd?.Invoke();
    }

    public void Skill() {
        OnSkill?.Invoke();
    }

    public void SkillEnd() {
        OnSkillEnd?.Invoke();
    }
}

