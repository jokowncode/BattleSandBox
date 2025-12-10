
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour {

    public static PoolManager Instance;

    private Dictionary<string, ObjectPool<PoolGO>> Pools = new Dictionary<string, ObjectPool<PoolGO>>();
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public PoolGO GetGameObject(PoolGO prefab) {
        if (!Pools.ContainsKey(prefab.PoolName)) {
            Pools.Add(prefab.PoolName, new ObjectPool<PoolGO>(() => Instantiate(prefab), (go) => {
                go.gameObject.SetActive(true);
                go.IsRelease = false;
            }, (go) => {
                go.gameObject.SetActive(false);
                go.IsRelease = true;
            }));
        }
        return Pools[prefab.PoolName].Get();
    }

    public void ReleaseGameObject(PoolGO go, float delay = 0.0f) {
        if (go.IsRelease) return;
        StartCoroutine(ReleaseGameObjectCoroutine(go, delay));
    }

    private IEnumerator ReleaseGameObjectCoroutine(PoolGO go, float delay) {
        if (delay > 0.01f) {
            yield return new WaitForSeconds(delay);
        }
        
        if (Pools.ContainsKey(go.PoolName)) {
            if(!go.IsRelease) Pools[go.PoolName].Release(go);
        } else {
            Destroy(go);
        }
    }
}


