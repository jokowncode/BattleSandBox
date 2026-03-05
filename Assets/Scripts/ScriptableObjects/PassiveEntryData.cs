
using UnityEngine;

public enum PassiveEntryRare {
    NormalPassiveEntry = GoodsType.普通芯片,
    SpecialPassiveEntry = GoodsType.特殊芯片,
}

[CreateAssetMenu(menuName = "DeckBreakers/PassiveEntryData", fileName = "PassiveEntryData")]
public class PassiveEntryData : ScriptableObject{
    public PassiveEntryRare Rare = PassiveEntryRare.NormalPassiveEntry;
    public string Name;
    public string Description;
    public PassiveEntrySort[] Sorts;
    public int Star = 1;

}
