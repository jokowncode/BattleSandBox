
using System;
using UnityEngine;

public class MagicCircle : MonoBehaviour{

    private bool IsHitTarget = false;
    private EffectData MagicCircleDamageMsg;
    private Vector3 TargetPos;
    
    public void SetDamageMessage(EffectData dm) {
        this.MagicCircleDamageMsg = dm;
    }

    public void SetTargetPos(Vector3 targetPos){
        this.TargetPos = targetPos;
    }

    private void Start(){
        this.transform.position = TargetPos;
        Destroy(this.gameObject, 1.5f);
    }

    private void OnTriggerEnter(Collider other){
        if (IsHitTarget) return;
        if (other.gameObject.layer != LayerMask.NameToLayer(this.MagicCircleDamageMsg.TargetType.ToString())) return;
        
        if (other.gameObject.TryGetComponent(out Fighter fighter)){
            IsHitTarget = true;
            fighter.BeDamaged(this.MagicCircleDamageMsg);
        }
    }
}

