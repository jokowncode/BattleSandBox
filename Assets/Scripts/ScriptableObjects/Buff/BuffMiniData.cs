using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewMiniBuff", menuName = "PhantomSpirit/Buff/Buff Mini Data")]
public class BuffMiniData : ScriptableObject {
    [Header("Property Change")]
    public BuffRef Ref;
    public PropertyRef PropertyRef;
    public FighterProperty CasterProperty;
    public FighterProperty TargetRefProperty;
    public FighterProperty TargetUpdateProperty;
    public PropertyModifyWay ModifyWay;
    public float ChangedValue;
    public bool IsChangeProperty = true; // For Health

    [Header("Particle")] 
    public bool IsDestroyImmediate = true;
    public float DestroyDelay = 0.5f;
    public GameObject EffectParticlePrefab;
}

