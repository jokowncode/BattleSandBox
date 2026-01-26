
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleImage : DialogUISlot {

    [SerializeField] private float MinScale = 1.0f;
    [SerializeField] private float MaxScale = 2.0f;
    [SerializeField] private float ZoomSpeed = 0.25f;
    [SerializeField] private float MoveSpeed = 0.25f;
    
    private RectTransform Trans;
    private float CurrentZoomDistance;

    private bool IsDrag = false;
    private Vector3 CurrentMousePosition;
    
    private void Awake() {
        this.Trans = this.GetComponent<RectTransform>();
        this.Init();
    }

    private void Update() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0.0f) {
            CurrentZoomDistance += scroll * this.ZoomSpeed;
            CurrentZoomDistance = Mathf.Clamp(CurrentZoomDistance, MinScale, MaxScale);
            this.Trans.localScale = CurrentZoomDistance * Vector3.one; 
        }

        if (Input.GetMouseButtonDown(0)) {
            this.IsDrag = true;
            this.CurrentMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) {
            this.IsDrag = false;
        }

        if (this.IsDrag) {
            Vector3 pos = Input.mousePosition;
            Vector3 delta = (pos - this.CurrentMousePosition).normalized;

            Vector3 transPos = this.Trans.position;
            transPos += this.MoveSpeed * delta;
            this.Trans.position = transPos;
            
            this.CurrentMousePosition = pos;
        }
    }

    public override void Init() {
        this.CurrentZoomDistance = this.MinScale;
        this.IsDrag = false;
    }

    public override void End() { }
}



