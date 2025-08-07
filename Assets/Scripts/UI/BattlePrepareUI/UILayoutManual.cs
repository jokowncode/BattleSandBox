using UnityEngine;
using System.Collections.Generic;

public class UILayoutManual : MonoBehaviour
{
    public float itemWidth = 100f;   // 每个 UI 的宽度（或估算值）
    public float spacing = 20f;      // UI 之间的间隔
    public float bottomOffset = 60f; // 距离屏幕底部的偏移

    private RectTransform parentRect;

    // void Start()
    // {
    //     LayoutChildren();
    // }

    public void LayoutChildren()
    {
        parentRect = GetComponent<RectTransform>();
        int childCount = parentRect.childCount;

        if (childCount == 0) return;

        float totalWidth = childCount * itemWidth + (childCount - 1) * spacing;
        float startX = -totalWidth / 2f + itemWidth / 2f;

        for (int i = 0; i < childCount; i++)
        {
            RectTransform child = parentRect.GetChild(i) as RectTransform;
            if (child == null) continue;

            // 锚点设置为中下
            child.anchorMin = new Vector2(0.5f, 0f);
            child.anchorMax = new Vector2(0.5f, 0f);
            child.pivot = new Vector2(0.5f, 0.5f);

            float x = startX + i * (itemWidth + spacing);
            float y = bottomOffset;

            child.anchoredPosition = new Vector2(x, y);
            
            // 自动添加 UIShake 脚本
            UIShaker shaker = child.GetComponent<UIShaker>();
            if (shaker == null)
            {
                shaker = child.gameObject.AddComponent<UIShaker>();
            }
        }
    }
}