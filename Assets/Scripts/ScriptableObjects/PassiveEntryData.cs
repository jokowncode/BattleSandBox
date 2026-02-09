
using UnityEngine;

public enum PassiveEntryRare {
    NormalPassiveEntry = GoodsType.普通词条,
    SpecialPassiveEntry = GoodsType.特殊词条,
}

[CreateAssetMenu(menuName = "DeckBreakers/PassiveEntryData", fileName = "PassiveEntryData")]
public class PassiveEntryData : ScriptableObject{
    public PassiveEntryRare Rare = PassiveEntryRare.NormalPassiveEntry;
    public string Name;
    public string Description;
    public PassiveEntrySort[] Sorts;
    public int Star = 1;

}
