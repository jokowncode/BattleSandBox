
using UnityEngine;

public class CampEquipmentInstruction : CampInstruction {

    [SerializeField] private DialogGraph InstructionDialog;
    
    protected override bool ShowCondition() {
        return SaveDataManager.Instance.DungeonIsComplete(SceneType.Dungeons_Newbie)
            && !SaveDataManager.Instance.DungeonIsComplete(SceneType.Dungeons_Level1);
    }

    protected override void AfterShow() {
        if (DialogManager.Instance && this.InstructionDialog) {
            DialogManager.Instance.OnDialogEnded += this.Disappear;
            DialogManager.Instance.PlayNewDialog(this.InstructionDialog);
        }
    }
}



