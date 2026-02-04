using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeHeroWarehouseUIManager : MonoBehaviour
{
    // ===== 模式枚举 =====
    public enum WarehouseMode
    {
        CharacterDisplay, // 角色展示
        CharacterBond     // 角色羁绊
    }

    [Header("当前模式")]
    [SerializeField] private WarehouseMode currentMode = WarehouseMode.CharacterDisplay;

    [Header("角色展示主面板")]
    [SerializeField] private GameObject displayPanelLeft;
    [SerializeField] private GameObject displayPanelRight;

    [Header("角色羁绊主面板")]
    [SerializeField] private GameObject bondPanelLeft;
    [SerializeField] private GameObject bondPanelRight;

    [Header("角色展示子面板")]
    [SerializeField] private GameObject battlePanel;
    [SerializeField] private GameObject voicePanel;
    [SerializeField] private GameObject storyPanel;

    [Header("角色展示区域")]
    [SerializeField] private Transform heroParent;
    [SerializeField] private List<GameObject> heroPrefabs;

    [Header("角色信息UI")]
    [SerializeField] private Text heroNameText;
    [SerializeField] private Text heroDescriptionText;
    [SerializeField] private Image heroImage;

    void Start()
    {
        // 默认打开角色展示模式，并显示左+右主面板，子面板默认战斗
        SwitchModeToCharacterDisplay();
        OpenBattlePanel();
    }

    // ===== 模式切换接口 =====
    public void SwitchModeToCharacterDisplay()
    {
        currentMode = WarehouseMode.CharacterDisplay;

        // 主面板显示
        if (displayPanelLeft != null) displayPanelLeft.SetActive(true);
        if (displayPanelRight != null) displayPanelRight.SetActive(true);

        // 羁绊面板隐藏
        if (bondPanelLeft != null) bondPanelLeft.SetActive(false);
        if (bondPanelRight != null) bondPanelRight.SetActive(false);

        // 默认子面板战斗
        OpenBattlePanel();

        Debug.Log("切换到角色展示模式");
    }

    public void SwitchModeToCharacterBond()
    {
        currentMode = WarehouseMode.CharacterBond;

        // 主面板隐藏
        if (displayPanelLeft != null) displayPanelLeft.SetActive(false);
        if (displayPanelRight != null) displayPanelRight.SetActive(false);

        // 羁绊面板显示
        if (bondPanelLeft != null) bondPanelLeft.SetActive(true);
        if (bondPanelRight != null) bondPanelRight.SetActive(true);

        // 子面板隐藏（羁绊暂不实现）
        if (battlePanel != null) battlePanel.SetActive(false);
        if (voicePanel != null) voicePanel.SetActive(false);
        if (storyPanel != null) storyPanel.SetActive(false);

        Debug.Log("切换到角色羁绊模式");
    }

    // ===== 角色获取和显示接口 =====
    public GameObject GetHero(int index)
    {
        if (heroPrefabs == null || index < 0 || index >= heroPrefabs.Count)
        {
            Debug.LogWarning("Hero prefab index out of range");
            return null;
        }

        if (heroParent == null)
        {
            Debug.LogWarning("heroParent 未设置");
            return null;
        }

        GameObject heroGO = Instantiate(heroPrefabs[index], heroParent);
        heroGO.name = heroPrefabs[index].name;
        return heroGO;
    }

    public void ShowHero(string heroName, string heroDescription, Sprite heroSprite)
    {
        if (heroNameText != null)
            heroNameText.text = heroName;

        if (heroDescriptionText != null)
            heroDescriptionText.text = heroDescription;

        if (heroImage != null)
            heroImage.sprite = heroSprite;
    }

    // ===== 子面板切换 =====
    public void OpenBattlePanel()
    {
        SetAllSubPanelsActive(false);
        if (battlePanel != null) battlePanel.SetActive(true);
    }

    public void OpenVoicePanel()
    {
        SetAllSubPanelsActive(false);
        if (voicePanel != null) voicePanel.SetActive(true);
    }

    public void OpenStoryPanel()
    {
        SetAllSubPanelsActive(false);
        if (storyPanel != null) storyPanel.SetActive(true);
    }

    private void SetAllSubPanelsActive(bool active)
    {
        if (battlePanel != null) battlePanel.SetActive(active);
        if (voicePanel != null) voicePanel.SetActive(active);
        if (storyPanel != null) storyPanel.SetActive(active);
    }
}
