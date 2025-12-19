
using System;
using UnityEngine;

public class MagicCircle : MonoBehaviour{

    private bool IsHitTarget = false;
    private EffectData MagicCircleDamageMsg;
    
    public void SetDamageMessage(EffectData dm) {
        this.MagicCircleDamageMsg = dm;
    }

    public void Init() {
        this.IsHitTarget = false;
        // Destroy(this.gameObject, 1.5f);
        if (this.TryGetComponent(out PoolGO poolGO)) {
            PoolManager.Instance.ReleaseGameObject(poolGO, 1.5f);
        } else {
            Destroy(this.gameObject, 1.5f);
        }
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

