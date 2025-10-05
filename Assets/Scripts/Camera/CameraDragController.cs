using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragController : MonoBehaviour
{
    [Header("拖拽设置")]
    public float dragSpeed = 10.0f;
    public bool invertDrag = false;
    public bool enableMovement = true;
    
    [Header("移动范围限制")]
    public bool enableMovementLimits = true;
    public float minX = -20f;
    public float maxX = 20f;
    public float minZ = -20f;
    public float maxZ = 20f;
    
    [Header("滚轮缩放设置")]
    public float zoomSpeed = 10.0f;
    public float minZoomDistance = 20.0f;
    public float maxZoomDistance = 50.0f;
    public bool enableZoom = true;
    
    [Header("移动平滑度")]
    public float smoothTime = 0.3f;
    
    private Vector3 dragOrigin;
    private Vector3 cameraTargetPosition;
    private float currentZoomDistance;
    private Vector3 velocity = Vector3.zero;
    
    void Start()
    {
        // 初始化相机目标位置和当前缩放距离
        cameraTargetPosition = transform.position;
        currentZoomDistance = Vector3.Distance(transform.position, Vector3.zero);
    }
    
    void Update()
    {
        if (enableMovement) HandleMouseDrag();
        if (enableZoom) HandleMouseZoom();
        
        // 应用移动范围限制
        if (enableMovementLimits)
        {
            cameraTargetPosition.x = Mathf.Clamp(cameraTargetPosition.x, minX, maxX);
            cameraTargetPosition.z = Mathf.Clamp(cameraTargetPosition.z, minZ, maxZ);
        }
        
        // 平滑移动相机到目标位置
        transform.position = Vector3.SmoothDamp(transform.position, cameraTargetPosition, ref velocity, smoothTime);
    }
    
    void HandleMouseDrag()
    {
        // 按下鼠标右键开始拖拽
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
        
        // 按住鼠标右键进行拖拽
        if (Input.GetMouseButton(1))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            
            // 根据是否反转拖拽方向调整移动方向
            Vector3 move = new Vector3(-difference.x * dragSpeed, 0, -difference.y * dragSpeed);
            if (invertDrag) move = -move;
            
            // 计算新的相机目标位置
            cameraTargetPosition += move;
            
            dragOrigin = Input.mousePosition;
        }
    }
    
    void HandleMouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0.0f)
        {
            // 计算缩放方向（朝向或远离世界中心点）
            Vector3 zoomDirection = (cameraTargetPosition - Vector3.zero).normalized;
            
            // 调整缩放距离
            currentZoomDistance -= scroll * zoomSpeed;
            currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);
            
            // 计算新的相机位置
            cameraTargetPosition = Vector3.zero + zoomDirection * currentZoomDistance;
        }
    }
    
    // 设置移动速度
    public void SetDragSpeed(float speed)
    {
        dragSpeed = Mathf.Max(0.1f, speed);
    }
    
    // 设置缩放速度
    public void SetZoomSpeed(float speed)
    {
        zoomSpeed = Mathf.Max(0.1f, speed);
    }
    
    // 设置最小缩放距离
    public void SetMinZoomDistance(float distance)
    {
        minZoomDistance = Mathf.Max(0.1f, distance);
        currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);
    }
    
    // 设置最大缩放距离
    public void SetMaxZoomDistance(float distance)
    {
        maxZoomDistance = Mathf.Max(minZoomDistance + 0.1f, distance);
        currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);
    }
    
    // 设置移动范围限制
    public void SetMovementLimits(float minX, float maxX, float minZ, float maxZ)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minZ = minZ;
        this.maxZ = maxZ;
        enableMovementLimits = true;
        
        // 立即应用限制
        cameraTargetPosition.x = Mathf.Clamp(cameraTargetPosition.x, minX, maxX);
        cameraTargetPosition.z = Mathf.Clamp(cameraTargetPosition.z, minZ, maxZ);
    }
    
    // 启用/禁用移动限制
    public void EnableMovementLimits(bool enable)
    {
        enableMovementLimits = enable;
    }
    
    // 启用/禁用移动
    public void EnableMovement(bool enable)
    {
        enableMovement = enable;
    }
    
    // 启用/禁用缩放
    public void EnableZoom(bool enable)
    {
        enableZoom = enable;
    }
    
    // 直接设置相机位置
    public void SetCameraPosition(Vector3 position)
    {
        cameraTargetPosition = position;
        
        // 更新缩放距离
        currentZoomDistance = Vector3.Distance(cameraTargetPosition, Vector3.zero);
        
        // 应用移动范围限制
        if (enableMovementLimits)
        {
            cameraTargetPosition.x = Mathf.Clamp(cameraTargetPosition.x, minX, maxX);
            cameraTargetPosition.z = Mathf.Clamp(cameraTargetPosition.z, minZ, maxZ);
        }
    }
    
    // 获取当前相机位置
    public Vector3 GetCameraPosition()
    {
        return cameraTargetPosition;
    }
    
    // 可选的：在Scene视图中绘制辅助线
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, Vector3.zero);
        Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        
        // 绘制移动范围限制
        if (enableMovementLimits)
        {
            Gizmos.color = Color.cyan;
            Vector3 center = new Vector3((minX + maxX) / 2, 0, (minZ + maxZ) / 2);
            Vector3 size = new Vector3(maxX - minX, 0.1f, maxZ - minZ);
            Gizmos.DrawWireCube(center, size);
        }
    }
}