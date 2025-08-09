using UnityEngine;

/// <summary>
/// 当玩家进入此触发区域时，更改指定GameObject的Layer；离开时恢复为Default Layer。
/// </summary>
public class SwitchToBlur : MonoBehaviour
{
    [Tooltip("需要改变Layer的游戏对象")]
    public GameObject objectToChangeLayer;

    [Tooltip("要切换到的目标Layer的名称")]
    public string targetLayerName = "Blur";

    private int defaultLayer;

    private void Start()
    {
        // "Default" layer的索引是0，但使用NameToLayer更具可读性
        defaultLayer = LayerMask.NameToLayer("Default");

        if (objectToChangeLayer == null)
        {
            Debug.LogError("请在Inspector中指定 'objectToChangeLayer'。", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 确保是玩家进入，并且目标对象已设置
        if (objectToChangeLayer != null && other.CompareTag("Player"))
        {
            int targetLayer = LayerMask.NameToLayer(targetLayerName);
            if (targetLayer == -1)
            {
                Debug.LogError($"Layer '{targetLayerName}' 不存在。请前往 Edit > Project Settings > Tags and Layers 创建。", this);
            }
            else
            {
                objectToChangeLayer.layer = targetLayer;
                SetLayerRecursively(objectToChangeLayer.transform, targetLayer);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 确保是玩家离开，并且目标对象已设置
        if (objectToChangeLayer != null && other.CompareTag("Player"))
        {
            objectToChangeLayer.layer = defaultLayer;
            SetLayerRecursively(objectToChangeLayer.transform, defaultLayer);
        }
    }

    /// <summary>
    /// 递归地为GameObject及其所有子对象设置Layer。
    /// </summary>
    /// <param name="parent">父对象的Transform</param>
    /// <param name="layer">要设置的目标Layer</param>
    void SetLayerRecursively(Transform parent, int layer)
    {
        parent.gameObject.layer = layer;
        foreach (Transform child in parent)
        {
            SetLayerRecursively(child, layer);
        }
    }
}
