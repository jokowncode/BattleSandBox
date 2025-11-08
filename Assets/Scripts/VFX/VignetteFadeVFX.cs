
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteFadeVFX : VFXBase {

    [SerializeField] private float MaxIntensity = 0.5f;
    [SerializeField] private float Duration = 0.5f;
    
    private Volume VignetteVolume;
    private Vignette CurrentVignette;

    private void Awake() {
        VignetteVolume = GetComponent<Volume>();
        if (VignetteVolume.profile.TryGet(out Vignette vignette)) {
            this.CurrentVignette = vignette;
        }
        VignetteVolume.weight = 0.0f;
    }

    public override void StartVFX() {
        VignetteVolume.weight = 1.0f;
        StartCoroutine(VignetteFadeCoroutine());
    }

    private IEnumerator VignetteFadeCoroutine() {
        float d = this.Duration / 2.0f;
        while (true) {
            for (float t = 0.0f; t < d; t += Time.deltaTime) {
                this.CurrentVignette.intensity.value = Mathf.Lerp(0.0f, this.MaxIntensity, t / d);
                yield return null;
            }
            this.CurrentVignette.intensity.value = this.MaxIntensity;
            for (float t = 0.0f; t < d; t += Time.deltaTime) {
                this.CurrentVignette.intensity.value = Mathf.Lerp(this.MaxIntensity, 0.0f, t / d);
                yield return null;
            }
            this.CurrentVignette.intensity.value = 0.0f;
        }
    }
}


