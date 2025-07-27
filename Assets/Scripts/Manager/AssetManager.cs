using UnityEngine;

public class AssetManager : MonoBehaviour {

    public static AssetManager Instance;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // TODO
    public void UnloadAsset(string assetRef) { }
    public T LoadAsset<T>(string assetRef) where T : Object { return null; }
}
