
using System;
using System.Collections.Generic;
using UnityEngine;

public class InPlaceSkillDelivery : SkillDelivery{
    private void Start(){
        this.transform.position = this.TargetPosition;
        Destroy(gameObject, 2.0f);
    }

    protected override void TriggerTarget(Collider other){
        if (other.gameObject.TryGetComponent(out Fighter fighter)) {
            this.Effect.ApplyEffect(fighter, this.EffectData);
        }
        Destroy(gameObject, 2.0f);
    }
}


