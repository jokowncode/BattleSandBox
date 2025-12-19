using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewMiniBuff", menuName = "PhantomSpirit/Buff/Buff Mini Data")]
public class BuffMiniData : ScriptableObject {
    public BuffRef Ref;
    public PropertyRef PropertyRef;
    public FighterProperty CasterProperty;
    public FighterProperty TargetRefProperty;
    public FighterProperty TargetUpdateProperty;
    public PropertyModifyWay ModifyWay;
    public float ChangedValue;
    public bool IsChangeProperty = true; // For Health

    public bool IsDestroyImmediate = true;
    public float DestroyDelay = 0.5f;
    public PoolGO EffectParticlePrefab;
    public bool EffectParticleGenerateCenter = true;
}

