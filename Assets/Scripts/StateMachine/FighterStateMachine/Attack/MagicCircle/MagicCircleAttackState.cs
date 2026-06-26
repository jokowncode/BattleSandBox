
using UnityEngine;

public class MagicCircleAttackState : AttackState{

    [SerializeField] private PoolGO MagicCirclePrefab;

    protected void CastMagicCircle(Transform target, float percentage = 1.0f){
        // MagicCircle magicCircle = Instantiate(this.MagicCirclePrefab);

        PoolGO go = PoolManager.Instance.GetGameObject(this.MagicCirclePrefab, target);
        if (!go.TryGetComponent(out MagicCircle magicCircle)) return;
        magicCircle.Init();
        
        EffectData damageMsg = GetEffectData();
        damageMsg.Value *= percentage;
        magicCircle.SetDamageMessage(damageMsg);
#if DEBUG_MODE
        Debug.Log($"{this.gameObject.name} Attack(magic circle) : {damageMsg.Value}");
        Controller.TotalDamage += damageMsg.Value;
#endif
    }
}

