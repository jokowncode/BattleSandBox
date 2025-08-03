using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BasicRef
{
    Caster,
    Target,
    Constant
}

[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff System/Buff Data")]
public class BuffData : ScriptableObject
{
    public string buffName;
    [TextArea] public string description;
    
    [Header("时间设置")]
    public float duration = 5f;       // Buff持续时间
    public float tickInterval = 1f;   // 效果触发间隔（0表示只触发一次）
    
    [Header("效果设置")]
    public List<BuffMiniData> immediateEffectBuff; // 立即触发效果
    public List<BuffMiniData> longTimeEffectBuff;// 持续触发效果
    public List<BuffMiniData> lastEffectBuff; // 最后额外效果
    
    // [Header("视觉效果")]
    // public ParticleSystem visualEffectPrefab;
    // public Material materialOverride;
}
