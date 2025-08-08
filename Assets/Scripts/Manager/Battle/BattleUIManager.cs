
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour {

    public static BattleUIManager Instance;

    [field: SerializeField] public Canvas UICanvas{ get; private set; }
    [SerializeField] private Image GameEndBannarImage;
    [SerializeField] private GameObject GameEndObject;

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
    
    [SerializeField] private TextMeshProUGUI SkillDescription;
    [SerializeField] private TextMeshProUGUI TalentDescription;
    [SerializeField] private TextMeshProUGUI PassiveSkill1Description;
    [SerializeField] private TextMeshProUGUI PassiveSkill2Description;

    private Image Skill1Image;
    private Image Skill2Image;
    private Image SkillBackgroundImage;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        SkillBackgroundImage = skillImageUI.GetComponentInChildren<Image>();
        Skill1Image = skill1UI.GetComponent<Image>();
        Skill2Image = skill2UI.GetComponent<Image>();
    }
    
    public void GameEnd(Sprite bannarSprite){
        this.GameEndBannarImage.sprite = bannarSprite;
        this.GameEndObject.SetActive(true);
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
        PassiveSkill1Description.text = text;
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
        PassiveSkill2Description.text = text;
        Color color = Skill2Image.color;
        if (text != ""){
            Skill2Image.color = new Color(color.r, color.g, color.b, 255);
        }else{
            Skill2Image.color =  new Color(color.r, color.g, color.b, 0);
        }
    }

    public void UpdateSelectedHeroSkillUI(FighterType type,string skillDesc, string talentDesc){
        if(type == FighterType.Mage)
            SkillBackgroundImage.sprite = mageSkillIcon;
        else if(type == FighterType.Priest)
            SkillBackgroundImage.sprite = priestSkillIcon;
        else
            SkillBackgroundImage.sprite = warriorSkillIcon;
        SkillDescription.text = skillDesc;
        TalentDescription.text = talentDesc;
    }
    
}

