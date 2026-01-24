
using UnityEngine;

[CreateAssetMenu(menuName = "PhantomSpirit/ClueData", fileName = "ClueData")]
public class ClueData : ScriptableObject {
    public string ClueName;
    public ClueType Type;
    [TextArea] public string ClueDescription;
}


