
using UnityEngine;

public class MagicCircleAttackState : AttackState{

    [SerializeField] private MagicCircle MagicCirclePrefab;

    protected override void Awake(){
        base.Awake();
        IsNeedTarget = false;
    }

    protected void CastMagicCircle(Vector3 position, float percentage){
        MagicCircle magicCircle = Instantiate(this.MagicCirclePrefab);
        magicCircle.SetTargetPos(position);
        
        float critical = Random.value < Controller.Critical / 100.0f ? 1.5f : 1.0f;
        
        EffectData damageMsg = new EffectData{
            Value = Controller.MagicAttack * critical * percentage,
            Force = Controller.Force,
            TargetType = Controller.AttackTargetType
        };
        magicCircle.SetDamageMessage(damageMsg);
#if DEBUG_MODE
        Debug.Log($"{this.gameObject.name} Attack(magic circle) : {damageMsg.Value}");
        Controller.TotalDamage += damageMsg.Value;
#endif
    }
}

