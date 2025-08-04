
using System;
using System.Collections;
using UnityEngine;

public class FighterRenderer : MonoBehaviour{

    [SerializeField] private float Duration = 0.5f;
    
    private SpriteRenderer Renderer;

    private bool IsChangeColor = false;
    
    private void Awake(){
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color color){
        if (IsChangeColor) return;
        StartCoroutine(ChangeColorCoroutine(color));
    }
    
    
    private IEnumerator ChangeColorCoroutine(Color color){
        IsChangeColor = true;
        Color originColor = this.Renderer.color;
        yield return FadeColorCoroutine(color, this.Duration / 2.0f);
        yield return FadeColorCoroutine(originColor, this.Duration / 2.0f);
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
}

