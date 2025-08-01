using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    void OnMouseDown()
    {
        // 当被点击时，通知管理器选中自己
        //HeroDetailUI.Instance.SelectObject(this);
    }

    // 选中时被调用
    public void OnSelected()
    {
        Debug.Log($"{name} 被选中了！");
        // 可以播放选中特效、声音等
    }

    // 点击按钮时被调用
    public void OnButtonClicked()
    {
        Debug.Log($"{name} 的按钮被点击了！");
        // 实现物体自己的按钮行为，比如升级、显示详情等
    }
}
