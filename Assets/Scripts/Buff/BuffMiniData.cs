using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff System/Buff Data")]
public class BuffMiniData : ScriptableObject
{
    public string buffName;
    [TextArea] public string description;
    
    [Header("数值参考")]
    public BasicRef basicRef; // 效果触发间隔（0表示只触发一次）
    
    [Header("效果设置")]
    public bool isRemoveBuff;
    public PropertyModifyWay propertyModifyWay;
    public float healthChangePerTickValue = 0.0f; // 每次触发的生命值变化
    public float speedChangeValue = 0.0f;// 速度乘数
    public float damageChangeValue = 0.0f;// 伤害乘数

    [Header("数值储存")] 
    private float healthValue;
    private float speedValue;
    private float damageValue;
    
    
}

