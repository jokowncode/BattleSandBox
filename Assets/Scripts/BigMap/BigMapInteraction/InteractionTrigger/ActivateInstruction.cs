
using UnityEngine;

public class ActivateInstruction : InteractionTrigger {

    [SerializeField] private InstructionType Type = InstructionType.Clue;
    
    protected override void TriggerAction() {
        if (BigMapUIManager.Instance) BigMapUIManager.Instance.ShowInstruction(this.Type);
    }
}

