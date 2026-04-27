
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstructionMask : MonoBehaviour, IPointerClickHandler, ICanvasRaycastFilter {
    
    private Material MaskMaterial;
    private RectTransform MaskTransform;

    public Action OnInstructionMaskClicked;

    private bool IsClickCanHide = true;
    private Image MaskImage;
    
    public void Show(RectTransform targetRect, Vector4 size, bool isClickCanHide) {
        this.IsClickCanHide = isClickCanHide;
        if (this.TryGetComponent(out Image image)) {
            this.MaskImage = image;
        }

        if (!this.MaskMaterial && this.MaskImage) {
            MaskMaterial = Instantiate(this.MaskImage.material);
            MaskMaterial.SetColor(MaterialProperty.Color, this.MaskImage.color);
            this.MaskImage.material = MaskMaterial;
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

    public bool IsRaycastLocationValid(Vector2 sp, Camera _) {
        if (this.IsClickCanHide) return true;
        RectTransform rectTrans = this.MaskImage.rectTransform;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTrans, sp, null, out Vector2 localPoint)) {
            return false;
        }

        Vector2 center = this.MaskMaterial.GetVector(MaterialProperty.Center);   
        Vector2 size = this.MaskMaterial.GetVector(MaterialProperty.Size);       
        Vector2 delta = localPoint - center;
        return Mathf.Abs(delta.x) >= size.x * 0.4f || Mathf.Abs(delta.y) >= size.y * 0.4f; 
    }
}


