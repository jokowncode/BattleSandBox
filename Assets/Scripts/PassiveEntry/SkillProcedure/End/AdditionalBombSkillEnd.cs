
using System;
using UnityEngine;

public class AdditionalBombSkillEnd : SkillEnd {

    [SerializeField] private ParticleSystem BombParticlePrefab;
    [SerializeField] private float BombValuePercentage = 0.4f;

    private void OnValidate() {
        BombValuePercentage = Mathf.Max(BombValuePercentage, 0.0f);
    }

    public override void AdditionalProcedure(Fighter influenceFighter, SkillEffect _, EffectData effectData) {
        Instantiate(BombParticlePrefab, influenceFighter.transform.position, Quaternion.identity);
        effectData.Force = 0.0f;
        effectData.Value *= BombValuePercentage;
        influenceFighter.BeDamaged(effectData);
    }
}

