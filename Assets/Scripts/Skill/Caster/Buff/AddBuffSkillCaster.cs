
using Unity.VisualScripting;
using UnityEngine;

public abstract class AddBuffSkillCaster : SkillCaster {
    
    [SerializeField] private BuffData BuffData;
    [SerializeField] private GameObject immediateEffectPrefab;  // 立即效果粒子预制体
    [SerializeField] private GameObject tickEffectPrefab;

    protected abstract override void Cast(Transform attackTarget);

    protected void AddBuff(Fighter ft) {
        if (!ft.TryGetComponent(out Buff buff)) {
            buff = ft.AddComponent<Buff>();
        }

        if(immediateEffectPrefab!=null)
            buff.immediateEffectPrefab = immediateEffectPrefab;
        if(tickEffectPrefab!=null)
            buff.tickEffectPrefab = tickEffectPrefab;
        buff.AddBuff(this.OwnedFighter, ft, BuffData);
    }
}

