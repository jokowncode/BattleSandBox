
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour {

    public static BattleUIManager Instance;

    [field: SerializeField] public Canvas UICanvas{ get; private set; }

    [field: SerializeField] public HeroWarehouseUI heroWarehouseUI{ get; private set; }
    [field: SerializeField] public PassiveEntryWarehouseUI PassiveEntryWarehouseUI{ get; private set; }
    [field: SerializeField] public HeroDetailUI heroDetailUI{ get; private set; }
    [field: SerializeField] public HeroPortraitUI heroPortraitUI{ get; private set; }

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

    private Image Skill1Image;
    private Image Skill2Image;
    private Image SkillBackgroundImage;
    
    private TextMeshProUGUI Skill1Text;
    private TextMeshProUGUI Skill2Text;
    private TextMeshProUGUI AttackDescText;
    private TextMeshProUGUI SkillDescText;
    
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        SkillBackgroundImage = skillImageUI.GetComponentInChildren<Image>();
        Skill1Image = skill1UI.GetComponent<Image>();
        Skill2Image = skill2UI.GetComponent<Image>();
        Skill1Text = passiveSkill1Description.GetComponent<TextMeshProUGUI>();
        Skill2Text = passiveSkill2Description.GetComponent<TextMeshProUGUI>();
        AttackDescText = attackDescription.GetComponent<TextMeshProUGUI>();
        SkillDescText = activeSkillDescription.GetComponent<TextMeshProUGUI>();
    }
    
    private void Start() {
        heroWarehouseUI.UpdateHeroWarehouse();
        PassiveEntryWarehouseUI.UpdateHeroWarehouse();
    }

    public void SetHeroWarehouseActive(bool active)
    {
        heroWarehouseUI.gameObject.SetActive(active);
    }

    public void SetSkillWarehouseActive(bool active)
    {
        PassiveEntryWarehouseUI.gameObject.SetActive(active);
    }

    public void SetHeroPanelActive(bool active)
    {
        heroDetailUI.gameObject.SetActive(active);
    }

    public void SetHeroPortraitActive(bool active)
    {
        heroPortraitUI.gameObject.SetActive(active);
    }

    public void SetSkillsNull()
    {
        SetSkill1UIText("Slot1");
        SetSkill2UIText("Slot2");
    }
    
    /// <summary>
    /// 修改 skill1UI 下的 TextMeshProUGUI 文本内容
    /// </summary>
    public void SetSkill1UIText(string text){
        Skill1Text.text = text;
        Color color = Skill1Image.color;
        if (text != ""){
            Skill1Image.color = new Color(color.r, color.g, color.b, 255);
        }else{
            Skill1Image.color =  new Color(color.r, color.g, color.b, 0);
        }
    }

    /// <summary>
    /// 修改 skill2UI 下的 TextMeshProUGUI 文本内容
    /// </summary>
    public void SetSkill2UIText(string text){
        Skill2Text.text = text;
        Color color = Skill2Image.color;
        if (text != ""){
            Skill2Image.color = new Color(color.r, color.g, color.b, 255);
        }else{
            Skill2Image.color =  new Color(color.r, color.g, color.b, 0);
        }
    }

    public void UpdateSelectedHeroSkillUI(FighterType type,string skillDesc = null){
        if(type == FighterType.Mage)
            SkillBackgroundImage.sprite = mageSkillIcon;
        else if(type == FighterType.Priest)
            SkillBackgroundImage.sprite = priestSkillIcon;
        else
            SkillBackgroundImage.sprite = warriorSkillIcon;
        AttackDescText.text = "普通攻击";
        SkillDescText.text = skillDesc;
    }
    
}

