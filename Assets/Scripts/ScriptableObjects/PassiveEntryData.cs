
using UnityEngine;

public enum PassiveEntryRare {
    NormalPassiveEntry = GoodsType.NormalPassiveEntry,
    SpecialPassiveEntry = GoodsType.SpecialPassiveEntry,
}

[CreateAssetMenu(menuName = "DeckBreakers/PassiveEntryData", fileName = "PassiveEntryData")]
public class PassiveEntryData : ScriptableObject{
    public PassiveEntryRare Rare = PassiveEntryRare.NormalPassiveEntry;
    public string Name;
    public string Description;
    public PassiveEntrySort[] Sorts;
    public int Star = 1;

}
