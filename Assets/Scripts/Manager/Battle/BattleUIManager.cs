
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour {

    public static BattleUIManager Instance;

    // TODO:change UI detail when selected hero
    public GameObject selectedGameObject;
    public Action OnSelected;
    
    [SerializeField] private HeroWarehouseUI heroWarehouseUI;
    [SerializeField] private SkillWarehouseUI skillWarehouseUI;
    [SerializeField] private HeroDetailUI heroDetailUI;
    
    [Header("Skill UI")]
    public Sprite warriorSkillIcon;
    public Sprite mageSkillIcon;
    public Sprite priestSkillIcon;
    public Sprite passiveSkillIcon;
    
    [SerializeField] private GameObject skillImageUI;
    [SerializeField] private GameObject skill1UI;
    [SerializeField] private GameObject skill2UI;
    
    [SerializeField] private GameObject attackDescription;
    [SerializeField] private GameObject activeSkillDescription;
    [SerializeField] private GameObject passiveSkill1Description;
    [SerializeField] private GameObject passiveSkill2Description;
    

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
        passiveSkill1Description.GetComponent<TextMeshProUGUI>().text = text;
        Color color = skill1UI.GetComponent<Image>().color;
        if (text != "")
        {
            skill1UI.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 255);
        }
        else
        {
            skill1UI.GetComponent<Image>().color =  new Color(color.r, color.g, color.b, 0);
        }
    }

    /// <summary>
    /// 修改 skill2UI 下的 TextMeshProUGUI 文本内容
    /// </summary>
    public void SetSkill2UIText(string text)
    {
        passiveSkill2Description.GetComponent<TextMeshProUGUI>().text = text;
        Color color = skill2UI.GetComponent<Image>().color;
        if (text != "")
        {
            skill2UI.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 255);
        }
        else
        {
            skill2UI.GetComponent<Image>().color =  new Color(color.r, color.g, color.b, 0);
        }
    }
    
    /// <summary>
    /// 修改 skill1UI 下的 Image
    /// </summary>
    public void SetSkill1UIImage()
    {
        
    }

    /// <summary>
    /// 修改 skill2UI 下的 Image
    /// </summary>
    public void SetSkill2UIImage()
    {

    }

    public void UpdateSelectedHeroSkillUI(FighterType type,string skillDesc = null)
    {
        if(type == FighterType.Mage)
            skillImageUI.GetComponentInChildren<Image>().sprite = mageSkillIcon;
        else if(type == FighterType.Priest)
            skillImageUI.GetComponentInChildren<Image>().sprite = priestSkillIcon;
        else
            skillImageUI.GetComponentInChildren<Image>().sprite = warriorSkillIcon;
        attackDescription.GetComponent<TextMeshProUGUI>().text = "普通攻击";
        activeSkillDescription.GetComponent<TextMeshProUGUI>().text = skillDesc;
    }
    
}

