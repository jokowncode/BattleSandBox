using System.Collections.Generic;
using UnityEngine;

public class UISelectionManager : MonoBehaviour
{
    public static UISelectionManager Instance;

    [Header("UI Settings")]
    public BattleTacticUI thirdUI; // 提示 UI，当选中两个时显示

    private List<UISelectableShaker> selectedList = new List<UISelectableShaker>();
    public int SelectedSize => selectedList.Count;

    private float InitialFixedDeltaTime;
    
    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (thirdUI != null)
            thirdUI.Hide();
        
        this.InitialFixedDeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// 尝试选中 UI
    /// </summary>
    public bool TrySelect(UISelectableShaker ui) {
        if (selectedList.Contains(ui)) return false;
        if (BattleManager.Instance.IsGameOver) return false;
        if (!ui.IsAlive) return false;       // 死亡不可选
        if (selectedList.Count >= 2) return false;
        if (!BattleUIManager.Instance.heroPortraitUI.HeroEnergyIsFull(ui.CurrentHero.Name)) return false;
        if (!ui.HasTactic && selectedList.Count != 0) return false; 
        
        selectedList.Add(ui);
        if (selectedList.Count == 1) {
            // Select One Hero
            BattleUIManager.Instance.heroPortraitUI.SelectOneHero(ui.CurrentHero.Name);
        } else {
            // Select Two Hero
            BulletTime(true);
            BattleUIManager.Instance.heroPortraitUI.ShowHeroLinkTip(selectedList[0].CurrentHero.Name, selectedList[1].CurrentHero.Name);
            BattleUIManager.Instance.heroPortraitUI.DownAllPanel(true);
        }

        UpdateThirdUI();
        return true;
    }

    private void BulletTime(bool open) {
        if (open) {
            Time.timeScale = 0.5f;
        } else {
            Time.timeScale = 1.0f;
        }
        Time.fixedDeltaTime = this.InitialFixedDeltaTime * Time.timeScale;
    }


    /// <summary>
    /// 取消选中 UI
    /// </summary>
    public void Unselect(UISelectableShaker ui) {
        if (!selectedList.Contains(ui)) return;
        selectedList.Remove(ui);
        
        if (selectedList.Count == 0) {
            BattleUIManager.Instance.heroPortraitUI.DownAllPanel(false);
        }

        if (selectedList.Count == 1) {
            if(!selectedList[0].IsSelected) selectedList[0].BeSelected();
            else BattleUIManager.Instance.heroPortraitUI.SelectOneHero(selectedList[0].CurrentHero.Name);
        }

        if (selectedList.Count != 2) {
            BattleUIManager.Instance.heroLinkTipUI.Hide();
            BulletTime(false);
        }
        UpdateThirdUI();
    }

    public void UnSelectAll() {
        if (selectedList.Count == 0) return;
        selectedList.Clear();
        UpdateThirdUI();
        BulletTime(false);
        BattleUIManager.Instance.heroLinkTipUI.Hide();
    }

    /// <summary>
    /// 更新第三个 UI 显示/隐藏
    /// </summary>
    private void UpdateThirdUI() {
        if (!thirdUI) return;
        bool showTactic = selectedList.Count == 2;
        if (!showTactic) {
            thirdUI.Hide();
            return;
        }
        thirdUI.Show(selectedList[0].CurrentHero.Name, selectedList[1].CurrentHero.Name);
    }

    public void UseTactic(BattleTacticType type) {
        if (selectedList.Count < 2) return;
        HeroMergeManager.Instance.MergeHeroTacticVersion(selectedList[0].CurrentHero, selectedList[1].CurrentHero, type);
        selectedList[0].GoDown(true);
        selectedList[1].GoDown(true);
        UnSelectAll();
    }

    /// <summary>
    /// 当前选中列表（只读）
    /// </summary>
    public List<UISelectableShaker> GetSelectedList()
    {
        return selectedList;
    }
}