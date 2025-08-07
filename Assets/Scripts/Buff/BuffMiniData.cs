using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBuff", menuName = "Buff System/Buff Mini Data")]
public class BuffMiniData : ScriptableObject
{
    public string buffName;
    [TextArea] public string description;
    
    [Header("数值参考")]
    public BasicRef basicRef; // 效果数据基数
    
    [Header("效果设置")]
    public bool isRemoveBuff;
    //public PropertyModifyWay propertyModifyWay;
    public FighterProperty refProperty;
    public FighterProperty changedProperty; // 伤害乘数
    public float changedValue;

    [Header("数值储存")]
    public float value;
}

