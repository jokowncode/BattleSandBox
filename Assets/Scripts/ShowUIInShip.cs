using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当玩家进入此触发区域时显示UI，离开时隐藏UI
/// </summary>
public class ShowUIInShip : MonoBehaviour
{
    [Tooltip("要显示的UI面板")]
    public GameObject uiPanel;

    [Tooltip("是否在开始时隐藏UI")]
    public bool hideOnStart = true;

    private void Start()
    {
        if (uiPanel == null)
        {
            Debug.LogError("请在Inspector中指定 'uiPanel'。", this);
            return;
        }

        // 如果设置了在开始时隐藏UI，则隐藏它
        if (hideOnStart)
        {
            uiPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 确保是玩家进入，并且UI面板已设置
        if (uiPanel != null && other.CompareTag("Player"))
        {
            uiPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 确保是玩家离开，并且UI面板已设置
        if (uiPanel != null && other.CompareTag("Player"))
        {
            uiPanel.SetActive(false);
        }
    }
}
