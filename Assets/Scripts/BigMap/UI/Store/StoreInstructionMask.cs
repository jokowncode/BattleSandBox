
using System;
using UnityEngine;
using UnityEngine.UI;

public class StoreInstructionMask : MonoBehaviour {
    
    private Material MaskMaterial;
    private RectTransform MaskTransform;
    
    private void Awake() {
        Image image = GetComponent<Image>();
        if (image) {
            MaskMaterial = Instantiate(image.material);
            MaskMaterial.SetColor(MaterialProperty.Color, image.color);
            image.material = MaskMaterial;
        }
        this.MaskTransform = this.GetComponent<RectTransform>();
        this.gameObject.SetActive(false);
    }

    public void Show(RectTransform targetRect, Vector4 size) {
        Vector3[] targetCorners = new Vector3[4];
        targetRect.GetWorldCorners(targetCorners);
        
        Vector3 worldCenter = (targetCorners[0] + targetCorners[2]) * 0.5f;
        Vector2 localCenter = MaskTransform.InverseTransformPoint(worldCenter);
        MaskMaterial.SetVector(MaterialProperty.Center, new Vector4(localCenter.x, localCenter.y, 0, 0));
        MaskMaterial.SetVector(MaterialProperty.Size, size);
        this.gameObject.SetActive(true);
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }
}


