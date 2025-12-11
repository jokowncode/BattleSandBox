
using System;
using System.Collections.Generic;
using UnityEngine;

public class InPlaceSkillDelivery : SkillDelivery{

    [SerializeField] private float DefaultDuration = 2.0f;

    private float Duration;
    private bool IsApplyEffect = false;

    protected override void PrepareDelivery() {
        this.IsApplyEffect = false;
        this.transform.position = this.TargetPosition;
        this.Duration = this.EffectData.Duration > 0.0f ? this.EffectData.Duration : this.DefaultDuration;
        // Destroy(gameObject, this.Duration);
        PoolManager.Instance.ReleaseGameObject(this.Effect.InPoolGO, this.Duration);
    }

    protected override void TriggerTargetIn(Collider other){
        if (this.IsApplyEffect) return;
        if (other.gameObject.TryGetComponent(out Fighter fighter)){
            this.IsApplyEffect = true;
            this.Effect.ApplyEffect(fighter, this.EffectData);
        }
    }
}


