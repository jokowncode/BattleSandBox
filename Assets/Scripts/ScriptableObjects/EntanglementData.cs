

using System;
using UnityEngine;

[Serializable]
public struct EntanglementPropertyChangeData {
    public FighterProperty ChangeProperty;
    public PropertyModifyWay ModifyWay;
    public float ChangeValue;
}

[CreateAssetMenu(menuName = "DeckBreakers/EntanglementData", fileName = "EntanglementData")]
public class EntanglementData : ScriptableObject {
    public float Value;
    public bool PropertyChange;
    public BattleTacticType CanUseMaxBattleTactic = BattleTacticType.None;
    public EntanglementPropertyChangeData[] PropertyChangeDatas;
}