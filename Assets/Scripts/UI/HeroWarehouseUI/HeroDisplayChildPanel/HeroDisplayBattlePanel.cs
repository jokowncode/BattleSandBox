
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroDisplayBattlePanel : HeroDisplayChildPanel {

    [Header("Property")] 
    [SerializeField] private TextMeshProUGUI Hp;
    [SerializeField] private TextMeshProUGUI PhysicsAttack;
    [SerializeField] private TextMeshProUGUI MagicAttack;
    [SerializeField] private TextMeshProUGUI Speed;
    [SerializeField] private TextMeshProUGUI Critical;
    [SerializeField] private TextMeshProUGUI Cooldown;

    [Header("Skill")] 
    [SerializeField] private Image SkillImage;
    [SerializeField] private TextMeshProUGUI SkillDesc;
    [SerializeField] private TextMeshProUGUI TalentDesc;
    
    // ReSharper disable Unity.PerformanceAnalysis
    protected override void ShowData(Hero hero) {
        this.Hp.text = hero.InitialHealth.ToString();
        this.PhysicsAttack.text = hero.InitialPhysicsAttack.ToString();
        this.MagicAttack.text = hero.InitialMagicAttack.ToString();
        this.Speed.text = hero.Speed.ToString();
        this.Critical.text = hero.InitialCritical.ToString();

        SkillCaster skillCaster = hero.GetComponentInChildren<SkillCaster>();
        if (skillCaster) {
            this.Cooldown.text = skillCaster.GetInitialData(SkillProperty.Cooldown).ToString();
            this.SkillDesc.text = skillCaster.SkillDesc;
        } else {
            this.Cooldown.text = "0";
            this.SkillDesc.text = "暂无";
        }
        this.SkillImage.sprite = hero.SkillPortrait;
        this.TalentDesc.text = hero.GetPassiveEntryDesc();
    }
}




