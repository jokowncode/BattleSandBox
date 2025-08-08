using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 当带有“Player”标签的对象进入此触发器时，调用在Inspector中指定的事件。
/// </summary>
public class GetinBattle : MonoBehaviour
{
    [Header("触发事件")]
    [Tooltip("当玩家进入此区域时要调用的事件。")]
    public UnityEvent onPlayerEnter;

    [Header("触发器设置")]
    [Tooltip("触发事件后是否禁用此脚本以防止重复触发。")]
    public bool disableAfterTriggering = true;

    private void OnTriggerEnter(Collider other)
    {
        // 检查进入的是否是玩家
        if (other.CompareTag("Player"))
        {
            // 调用在Inspector中设置的所有事件
            onPlayerEnter.Invoke();

            // 如果设置了触发后禁用，则禁用此脚本
            if (disableAfterTriggering)
            {
                // 禁用脚本本身，而不是整个GameObject
                this.enabled = false;
            }
        }
    }

    /// <summary>
    /// 在Inspector中为该组件添加一个上下文菜单项，用于方便地添加或检查碰撞体。
    /// </summary>
    [ContextMenu("Ensure Trigger Collider")]
    private void EnsureTriggerCollider()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            // 如果没有碰撞体，添加一个BoxCollider作为默认值
            col = gameObject.AddComponent<BoxCollider>();
            Debug.Log("已添加默认的BoxCollider。请根据需要调整其大小和形状。", this);
        }

        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.Log($"已将 {col.GetType().Name} 上的 'Is Trigger' 设置为 true。", this);
        }
        else
        {
            Debug.Log("碰撞体已经是触发器。", this);
        }
    }
}
