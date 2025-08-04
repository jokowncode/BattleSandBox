
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour {

    public static CameraManager Instance;

    private Camera MainCamera;

    private bool IsShake;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        MainCamera = Camera.main;
    }
    
    private IEnumerator ShakeCoroutine(float duration, float magnitude, float late){
        IsShake = true;
        if(late != 0.0f) yield return new WaitForSeconds(late);
        Vector3 originalPos = MainCamera.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration){
            float y = Random.Range(-1f, 1f) * magnitude;
            MainCamera.transform.localPosition = new Vector3(originalPos.x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        MainCamera.transform.localPosition = originalPos;
        IsShake = false;
    }
    
    public void ShakeCamera(float duration, float magnitude, float late = 0.0f){
        if (IsShake) return;
        StartCoroutine(ShakeCoroutine(duration, magnitude, late));
    }
}

