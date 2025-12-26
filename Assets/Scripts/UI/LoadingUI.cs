
using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour {

    [SerializeField] private Image LoadingImage;
    
    private Material LoadingMaterial;
    private CanvasGroup LoadingCanvasGroup;
    
    private void Awake(){
        Material mat = this.LoadingImage.material;
        this.LoadingMaterial = new Material(mat);
        this.LoadingImage.material = this.LoadingMaterial;
        
        this.LoadingCanvasGroup = this.GetComponent<CanvasGroup>();
    }

    public void Transition(bool show) {
        float alpha = show ? 1.0f : 0.0f;
        this.LoadingCanvasGroup.alpha = alpha;
    }

    public void UpdateLoadingProgress(float progress) {
        this.LoadingMaterial.SetFloat(MaterialProperty.LoadingPoint, progress);
    }
}

