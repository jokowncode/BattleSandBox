
using System.Collections.Generic;
using UnityEngine;

public class GetClue : InteractionTrigger {

    [ScriptableObjectNameProp(typeof(ClueData), "ClueName")]
    [SerializeField] private List<string> ClueNames;
    
    protected override void TriggerAction() {
        foreach (string clueName in ClueNames) {
            ClueWarehouseManager.Instance.AddClue(clueName);
        }
    }


}


