
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstructionMask : MonoBehaviour, IPointerClickHandler {
    
    private Material MaskMaterial;
    private RectTransform MaskTransform;

    public Action OnInstructionMaskClicked;

    [HideInInspector] public bool IsClickCanHide = true;
    
    public void Show(RectTransform targetRect, Vector4 size) {
        if (!this.MaskMaterial && this.TryGetComponent(out Image image)) {
            MaskMaterial = Instantiate(image.material);
            MaskMaterial.SetColor(MaterialProperty.Color, image.color);
            image.material = MaskMaterial;
        }

        if (!this.MaskTransform) {
            this.TryGetComponent(out this.MaskTransform);
        }
        
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

    public void OnPointerClick(PointerEventData eventData) {
        if (this.IsClickCanHide) this.Hide();
        OnInstructionMaskClicked?.Invoke();
    }
}


