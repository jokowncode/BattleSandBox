
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstructionMask : MonoBehaviour, ICanvasRaycastFilter {
    
    private Material MaskMaterial;
    private RectTransform MaskTransform;

    public Action OnInstructionMaskClicked;

    private Image MaskImage;
    
    public void Show(RectTransform targetRect, Vector4 size, bool isClickCanHide) {
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
        this.enabled = isClickCanHide;
    }

    public void Hide() {
        this.enabled = false;
        this.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            this.Hide();
            OnInstructionMaskClicked?.Invoke();
            
            List<RaycastResult> results = EventSystem.current.GetRaycastResult();
            foreach (var result in results) {
                if (result.gameObject.TryGetComponent(out Button button)) {
                    button.onClick.Invoke();
                    break;
                }
            }
        }
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera _) {
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


