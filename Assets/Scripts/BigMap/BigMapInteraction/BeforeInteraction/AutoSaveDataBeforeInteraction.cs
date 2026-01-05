using UnityEngine;

public class AutoSaveDataBeforeInteraction : MonoBehaviour {
    private void Awake() {
        if (this.TryGetComponent(out InteractionObject io)) {
            io.OnInteractionPre += () => {
                // TODO: Multi Save Data -> Auto Save
                SaveMapManager.Instance.SaveData();
            };
        }
    }
}
