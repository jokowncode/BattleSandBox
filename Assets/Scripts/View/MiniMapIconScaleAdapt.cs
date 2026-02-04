using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMapIconScaleAdapt : MonoBehaviour {

    [SerializeField] private bool TiledSprite = false;
    
    [SerializeField] private AnimationCurve HorizontalAdaptCurve;
    [SerializeField] private AnimationCurve VerticalAdaptCurve;
    
    private Camera MapCamera;
    private SpriteRenderer Renderer;
    
    private void Awake() {
        this.Renderer = this.GetComponent<SpriteRenderer>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        this.MapCamera = GameObject.FindWithTag("MapCamera").GetComponent<Camera>();
    }

    private void LateUpdate() {
        float scaleH = this.HorizontalAdaptCurve.Evaluate(this.MapCamera.orthographicSize);
        float scaleV = this.VerticalAdaptCurve.Evaluate(this.MapCamera.orthographicSize);
        
        if (this.TiledSprite) {
            this.Renderer.size = new Vector2(scaleH, scaleV);
        } else {
            transform.localScale = new Vector3(scaleH, scaleV, 1.0f);
        }
    }
}
