
using System;
using UnityEngine;
using TMPro;

public class BattleUIManager : MonoBehaviour {

    public static BattleUIManager Instance;

    // TODO:change UI detail when selected hero
    public GameObject selectedGameObject;
    public Action OnSelected;
    
    [SerializeField] private HeroWarehouseUI heroWarehouseUI;
    [SerializeField] private SkillWarehouseUI skillWarehouseUI;
    [SerializeField] private HeroDetailUI heroDetailUI;
    [SerializeField] private GameObject skill1UI;
    [SerializeField] private GameObject skill2UI;

    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start() {
        heroWarehouseUI.UpdateHeroWarehouse();
        skillWarehouseUI.UpdateHeroWarehouse();
    }

    public void SetHeroWarhouseActive(bool active)
    {
        heroWarehouseUI.gameObject.SetActive(active);
    }

    public void SetSkillWarehouseActive(bool active)
    {
        skillWarehouseUI.gameObject.SetActive(active);
    }

    public void SetHeroPanelActive(bool active)
    {
        heroDetailUI.gameObject.SetActive(active);
    }

    public void SetSkillsNull()
    {
        SetSkill1UIText("Slot1");
        SetSkill2UIText("Slot2");
    }
    
    /// <summary>
    /// 修改 skill1UI 下的 TextMeshProUGUI 文本内容
    /// </summary>
    public void SetSkill1UIText(string text)
    {
        TextMeshProUGUI tmp = skill1UI.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = text;
        }
        else
        {
            Debug.LogWarning("skill1UI 下未找到 TextMeshProUGUI");
        }
    }

    /// <summary>
    /// 修改 skill2UI 下的 TextMeshProUGUI 文本内容
    /// </summary>
    public void SetSkill2UIText(string text)
    {
        TextMeshProUGUI tmp = skill2UI.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = text;
        }
        else
        {
            Debug.LogWarning("skill2UI 下未找到 TextMeshProUGUI");
        }
    }
    
}

