
using UnityEngine;

public class MagicCircleAttackState : AttackState{

    [SerializeField] private MagicCircle MagicCirclePrefab;

    protected override void Awake(){
        base.Awake();
        IsNeedTarget = false;
    }

    protected void CastMagicCircle(Vector3 targetPos, float percentage){
        MagicCircle magicCircle = Instantiate(this.MagicCirclePrefab);
        magicCircle.SetTargetPos(targetPos);
        
        float critical = Random.value < Controller.Critical / 100.0f ? 1.5f : 1.0f;
        magicCircle.SetDamageMessage(new EffectData{
            Value = Controller.MagicAttack * critical * percentage,
            Force = Controller.Force,
            TargetType = Controller.AttackTargetType
        });
    }
}

