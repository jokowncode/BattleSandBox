
using UnityEngine;

public class StoreInstructionDialogEvent : MonoBehaviour {

    private void ShowStore() {
        BigMapUIManager.Instance.TransitionStore(true);
    }

    private void HideStore() {
        BigMapUIManager.Instance.TransitionStore(false);
    }

}
