
using UnityEngine;

[CreateAssetMenu(menuName = "DeckBreakers/PassiveEntryData", fileName = "PassiveEntryData")]
public class PassiveEntryData : ScriptableObject{

    public string Name;
    public string Description;
    public PassiveEntrySort[] Sorts;
    public int Star = 1;

}
