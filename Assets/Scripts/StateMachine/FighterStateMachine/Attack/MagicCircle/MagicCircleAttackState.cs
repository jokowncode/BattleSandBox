
using UnityEngine;

public class MagicCircleAttackState : AttackState{

    [SerializeField] private PoolGO MagicCirclePrefab;

    protected void CastMagicCircle(Transform target, float percentage){
        // MagicCircle magicCircle = Instantiate(this.MagicCirclePrefab);

        PoolGO go = PoolManager.Instance.GetGameObject(this.MagicCirclePrefab, target);
        if (!go.TryGetComponent(out MagicCircle magicCircle)) return;
        magicCircle.Init();
        
        bool criticalTest = Random.value < Controller.Critical / 100.0f;
        float critical = criticalTest ? 1.5f : 1.0f;
        
        EffectData damageMsg = new EffectData{
            Value = Controller.MagicAttack * critical * percentage,
            Force = Controller.Force,
            TargetType = Controller.AttackTargetType,
            IsCritical = criticalTest
        };
        magicCircle.SetDamageMessage(damageMsg);
#if DEBUG_MODE
        Debug.Log($"{this.gameObject.name} Attack(magic circle) : {damageMsg.Value}");
        Controller.TotalDamage += damageMsg.Value;
#endif
    }
}

