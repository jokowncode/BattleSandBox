

using System;
using UnityEngine;

public class TotalMapUI : MonoBehaviour {

    [SerializeField] private Camera MapCamera;
    [SerializeField] private float MinScale = 5.0f;
    [SerializeField] private float MaxScale = 30.0f;
    [SerializeField] private float InitialScale = 15.0f;
    [SerializeField] private float ScrollSpeed = 2.0f;
    [SerializeField] private AnimationCurve MoveSpeedAdaptCurve;
    
    private bool IsDrag = false;
    private Vector3 CurrentMousePosition;
    private float MapCameraOriginSize;

    public Action OnClose;

    public void Show() {
        SaveDataManager.Instance.PlayerInBigMap.TransMove(false);
        this.MapCameraOriginSize = this.MapCamera.orthographicSize;
        this.MapCamera.orthographicSize = this.InitialScale;
        this.MapCamera.transform.parent = null;
        this.gameObject.SetActive(true);
    }

    public void Hide() {
        SaveDataManager.Instance.PlayerInBigMap.TransMove(true);
        this.MapCamera.transform.parent = CameraManager.Instance.MainCamera.transform;
        this.MapCamera.transform.localPosition = Vector3.zero;
        this.MapCamera.orthographicSize = this.MapCameraOriginSize;
        this.IsDrag = false;
        OnClose?.Invoke();
        this.gameObject.SetActive(false);
    }

    private void Update() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f) {
            this.MapCamera.orthographicSize += -scroll * this.ScrollSpeed;
            this.MapCamera.orthographicSize = Mathf.Clamp(this.MapCamera.orthographicSize, this.MinScale, this.MaxScale);
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
            Vector3 delta = -(pos - this.CurrentMousePosition).normalized;

            Vector3 transPos = this.MapCamera.transform.position;
            transPos += this.MoveSpeedAdaptCurve.Evaluate(this.MapCamera.orthographicSize) * delta;
            this.MapCamera.transform.position = transPos;
            
            this.CurrentMousePosition = pos;
        }
    }
}


