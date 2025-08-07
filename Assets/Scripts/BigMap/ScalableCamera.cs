using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableCamera : MonoBehaviour
{
    [Header("Zoom Settings")]
    [Tooltip("鼠标滚轮缩放速度，建议取正值。")]
    public float scrollSpeed = 10f;

    [Tooltip("摄像机Z轴（本地坐标或世界坐标）最小值（最远，负数朝外）。")] public float minZ = -60f;
    [Tooltip("摄像机Z轴（本地坐标或世界坐标）最大值（最近）。")] public float maxZ = -7.5f;

    [Header("需要在缩放时禁用的脚本（例如玩家控制脚本）")]
    public MonoBehaviour[] scriptsToDisable;

    private Transform camTransform;

    void Start()
    {
        camTransform = transform;
        // 将摄像机Z轴初始化为默认值
        Vector3 pos = camTransform.localPosition;
        pos.z = maxZ;
        camTransform.localPosition = pos;
        // 初始时确保玩家输入开启
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
        pos.z += scroll * scrollSpeed; // 鼠标向前滚时scroll为正，向近处移动Z
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        camTransform.localPosition = pos;

        bool isDefault = Mathf.Approximately(pos.z, maxZ);
        SetPlayerInputEnabled(isDefault);
    }

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
