
using UnityEngine;

public class PoolGO : MonoBehaviour {
    [field: SerializeField] public string PoolName { get; private set; }
    public bool IsRelease { get; set; }

    private void LateUpdate() {
        this.transform.rotation = Quaternion.identity;
    }
}

