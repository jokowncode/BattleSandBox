
using System;
using System.Collections;
using UnityEngine;

public class FighterRenderer : MonoBehaviour{
    
    [SerializeField] private float ChangeColorDuration = 0.5f;
    [SerializeField] private float FlashingDuration = 0.5f;
    [SerializeField] private float DeadDissolveDuration = 3.0f;
    
    private SpriteRenderer Renderer;

    private bool IsChangeColor = false;
    private bool IsFlashing = false;
    private bool IsDead = false;
    
    private void Awake(){
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color color){
        if (IsChangeColor) return;
        StartCoroutine(ChangeColorCoroutine(color));
    }

    public void Flash(){
        if (IsFlashing) return;
        StartCoroutine(FlashingCoroutine());
    }

    public void Dead(){
        if (IsDead) return;
        StartCoroutine(DeadCoroutine());
    }
    
    private IEnumerator DeadCoroutine(){
        IsDead = true;
        float currentDissolve = this.Renderer.material.GetFloat(MaterialProperty.Dissolve);
        for (float t = 0.0f; t < this.DeadDissolveDuration; t += Time.deltaTime){
            this.Renderer.material.SetFloat(MaterialProperty.Dissolve, Mathf.Lerp(currentDissolve, 1.0f, t / DeadDissolveDuration));
            yield return null;
        }
        this.Renderer.material.SetFloat(MaterialProperty.Dissolve, 1.0f);
        IsDead = false;
        // Destroy(this.transform.parent.gameObject);
    }

    private IEnumerator FlashingCoroutine(){
        IsFlashing = true;
        float originFlash = this.Renderer.material.GetFloat(MaterialProperty.Flashing);
        yield return FadeFlashingCoroutine(1.0f, this.FlashingDuration / 2.0f);
        yield return FadeFlashingCoroutine(originFlash, this.FlashingDuration / 2.0f);
        IsFlashing = false;
    }
    
    private IEnumerator ChangeColorCoroutine(Color color){
        IsChangeColor = true;
        Color originColor = this.Renderer.color;
        yield return FadeColorCoroutine(color, this.ChangeColorDuration / 2.0f);
        yield return FadeColorCoroutine(originColor, this.ChangeColorDuration / 2.0f);
        IsChangeColor = false;
    }

    private IEnumerator FadeColorCoroutine(Color newColor, float duration){
        Color startColor = this.Renderer.color;
        for (float t = 0.0f; t < duration; t += Time.deltaTime){
            this.Renderer.color = Color.Lerp(startColor, newColor, t / duration);
            yield return null;
        }
        this.Renderer.color = newColor;
    }
    
    private IEnumerator FadeFlashingCoroutine(float finalFlash, float duration){
        float currentFlash = this.Renderer.material.GetFloat(MaterialProperty.Flashing);
        for (float t = 0.0f; t < duration; t += Time.deltaTime){
            this.Renderer.material.SetFloat(MaterialProperty.Flashing, Mathf.Lerp(currentFlash, finalFlash, t / duration));
            yield return null;
        }
        this.Renderer.material.SetFloat(MaterialProperty.Flashing, finalFlash);
    }
}

