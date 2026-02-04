

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "DeckBreakers/EntanglementData", fileName = "EntanglementData")]
public class EntanglementData : ScriptableObject {
    public float Value;
    public bool PropertyChange;
    public BattleTacticType CanUseMaxBattleTactic = BattleTacticType.None;
    
    public FighterProperty ChangeProperty;
    public PropertyModifyWay ModifyWay;
    public float ChangeValue;
}