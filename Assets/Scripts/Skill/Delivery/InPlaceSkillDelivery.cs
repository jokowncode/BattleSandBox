
using System;
using UnityEngine;

public class InPlaceSkillDelivery : SkillDelivery {
    private void Start(){
        this.transform.position = this.TargetPosition;
        Destroy(gameObject, 1.0f);
    }

    protected override void TriggerTarget(Collider other){
        if (other.gameObject.TryGetComponent(out Fighter fighter)) {
            this.Effect.ApplyEffect(fighter, this.EffectData);
        }
        Destroy(gameObject, 0.5f);
    }
}


