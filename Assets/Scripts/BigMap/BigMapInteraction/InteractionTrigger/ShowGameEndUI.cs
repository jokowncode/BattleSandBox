
using UnityEngine;

public class ShowGameEndUI : InteractionTrigger {
    
    protected override void TriggerAction() {
        GameEndUI.Instance.Show();
    }
}



