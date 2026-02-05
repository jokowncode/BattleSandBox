using System.Collections.Generic;
using UnityEngine;

public class UISelectionManager : MonoBehaviour
{
    public static UISelectionManager Instance;

    [Header("UI Settings")]
    public GameObject thirdUI; // 提示 UI，当选中两个时显示

    private List<UISelectableShaker> selectedList = new List<UISelectableShaker>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (thirdUI != null)
            thirdUI.SetActive(false);
    }

    /// <summary>
    /// 尝试选中 UI
    /// </summary>
    public bool TrySelect(UISelectableShaker ui)
    {
        if (!ui.IsAlive) return false;       // 死亡不可选
        if (selectedList.Count >= 2) return false;
        if (!selectedList.Contains(ui))
            selectedList.Add(ui);
        UpdateThirdUI();
        return true;
    }


    /// <summary>
    /// 取消选中 UI
    /// </summary>
    public void Unselect(UISelectableShaker ui)
    {
        if (selectedList.Contains(ui))
            selectedList.Remove(ui);

        UpdateThirdUI();
    }

    /// <summary>
    /// 更新第三个 UI 显示/隐藏
    /// </summary>
    private void UpdateThirdUI()
    {
        if (thirdUI != null)
            thirdUI.SetActive(selectedList.Count == 2);
    }

    /// <summary>
    /// 当前选中列表（只读）
    /// </summary>
    public List<UISelectableShaker> GetSelectedList()
    {
        return selectedList;
    }
}