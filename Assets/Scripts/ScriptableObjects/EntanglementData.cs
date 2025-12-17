

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PhantomSpirit/EntanglementData", fileName = "EntanglementData")]
public class EntanglementData : ScriptableObject {
    public float Value;
    public bool PropertyChange;

    public FighterProperty ChangeProperty;
    public PropertyModifyWay ModifyWay;
    public float ChangeValue;
}