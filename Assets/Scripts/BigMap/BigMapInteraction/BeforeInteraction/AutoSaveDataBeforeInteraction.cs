using UnityEngine;

public class AutoSaveDataBeforeInteraction : MonoBehaviour {
    private void Awake() {
        if (this.TryGetComponent(out InteractionObject io)) {
            io.OnInteractionPre += () => {
                SaveMapManager.Instance.SaveData();
            };
        }
    }
}
