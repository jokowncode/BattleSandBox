using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff System/Buff Data")]
public class BuffData : ScriptableObject
{
    public string buffName;
    [TextArea] public string description;
    
    [Header("时间设置")]
    public float duration = 5f;       // Buff持续时间
    public float tickInterval = 1f;   // 效果触发间隔（0表示只触发一次）
    
    [Header("效果设置")]
    public bool isRemoveBuff;
    public float healthChangePerTick; // 每次触发的生命值变化
    public float speedMultiplier = 1f;// 速度乘数
    public float damageMultiplier = 1f;// 伤害乘数
    
    // [Header("视觉效果")]
    // public ParticleSystem visualEffectPrefab;
    // public Material materialOverride;
}
