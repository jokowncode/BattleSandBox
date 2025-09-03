
using Unity.VisualScripting;
using UnityEngine;

public abstract class AddBuffSkillCaster : SkillCaster {
    
    [SerializeField] private BuffData BuffData;

    protected abstract override void Cast(Transform attackTarget);

    protected void AddBuff(Fighter ft) {
        BuffManager.Instance.AddBuff(this.OwnedFighter, ft, BuffData);
    }
}

