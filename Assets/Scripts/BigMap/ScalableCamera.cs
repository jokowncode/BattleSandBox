using UnityEngine;

public class ScalableCamera : MonoBehaviour
{
    [Header("Zoom Settings")]
    [Tooltip("鼠标滚轮缩放速度。")]
    public float scrollSpeed = 10f;
    [Tooltip("摄像机Z轴最小值（最远）。")]
    public float minZ = -60f;
    [Tooltip("摄像机Z轴最大值（最近）。")]
    public float maxZ = -7.5f;

    [Header("UI Control Settings")]
    [Tooltip("需要控制透明度的UI面板（需挂载CanvasGroup组件）。")]
    public CanvasGroup panelToControl;
    [Tooltip("UI淡入淡出范围（0到1）。0.2表示在从最近点开始的20%缩放距离内完成淡入淡出。")]
    [Range(0.01f, 1f)]
    public float fadeRange = 0.2f;

    [Header("Scripts To Disable On Zoom")]
    [Tooltip("在缩放时需要禁用的脚本。")]
    public MonoBehaviour[] scriptsToDisable;

    private Transform camTransform;

    void Start()
    {
        camTransform = transform;
        Vector3 pos = camTransform.localPosition;
        pos.z = maxZ;
        camTransform.localPosition = pos;
        
        UpdatePanelAlpha(pos.z);
        SetPlayerInputEnabled(true);
    }

    void Update()
    {
        HandleScroll();
    }

    private void HandleScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Approximately(scroll, 0f)) return;

        Vector3 pos = camTransform.localPosition;
        pos.z += scroll * scrollSpeed;
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        camTransform.localPosition = pos;

        UpdatePanelAlpha(pos.z);

        bool isAtDefaultZoom = Mathf.Approximately(pos.z, maxZ);
        SetPlayerInputEnabled(isAtDefaultZoom);
    }

    /// <summary>
    /// 根据摄像机的Z轴位置和淡入淡出范围更新UI面板的透明度。
    /// </summary>
    /// <param name="currentZ">当前的Z轴位置。</param>
    private void UpdatePanelAlpha(float currentZ)
    {
        if (panelToControl == null) return;

        // 计算总的缩放距离
        float totalZoomDistance = minZ - maxZ;
        // 定义淡入淡出真正发生的Z轴点
        float fadeStartZ = maxZ + totalZoomDistance * fadeRange;

        // 计算当前位置在淡入淡出范围内的归一化值
        // 如果 currentZ > fadeStartZ, 说明还在完全不透明的区域
        // 如果 currentZ < maxZ, 说明已经超出范围，应为完全透明
        float normalizedAlpha = (currentZ - maxZ) / (fadeStartZ - maxZ);
        
        // 我们希望是反向的，所以用1去减，并把结果限制在0-1之间
        panelToControl.alpha = Mathf.Clamp01(1 - normalizedAlpha);
    }

    /// <summary>
    /// 启用或禁用指定的脚本。
    /// </summary>
    private void SetPlayerInputEnabled(bool enabled)
    {
        if (scriptsToDisable == null) return;
        foreach (var behaviour in scriptsToDisable)
        {
            if (behaviour != null)
            {
                behaviour.enabled = enabled;
            }
        }
    }
}
