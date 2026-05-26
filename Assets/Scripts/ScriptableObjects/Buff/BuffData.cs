using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuff", menuName = "DeckBreakers/Buff/BuffData")]
public class BuffData : ScriptableObject {

    public CascadeBuffType CascadeType = CascadeBuffType.None;
    public int LimitCount = -1; // Buff最大叠加层数（-1表示不限）
    public bool IsDebuff = false; 
    
    [Header("时间设置")]
    public float Duration = 5f;       // Buff持续时间 （-1表示不限时间）
    public float TickInterval = 1f;   // 效果触发间隔（0表示只触发一次）

    [Header("持续性粒子")]
    public PoolGO ParticlePrefab;
    public bool ParticleGenerateCenter = true;
    
    [Header("效果设置")]
    public List<BuffMiniData> ImmediateEffectBuff; // 立即触发效果
    public List<BuffMiniData> LongTimeEffectBuff;// 持续触发效果
    public List<BuffData> LastEffectBuff; // 最后额外效果
}
