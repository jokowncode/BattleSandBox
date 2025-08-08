
using System;
using UnityEngine;

public class FlashChainDamage : MonoBehaviour{

    [SerializeField] private float Damage = 10.0f;
    
    public TargetType Type;
    
    private void OnTriggerEnter(Collider other){
        if (other.TryGetComponent(out Fighter fighter) && fighter.gameObject.layer == LayerMask.NameToLayer(Type.ToString())) {
            Debug.Log("aaa");
            fighter.BeDamaged(new EffectData{
                Value = Damage
            });
        }
    }
}
