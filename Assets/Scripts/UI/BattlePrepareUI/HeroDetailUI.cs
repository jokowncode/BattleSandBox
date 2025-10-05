using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HeroDetailUI : MonoBehaviour {
    
    [SerializeField] private Image heroImage;
    
    [Header("Detail")]
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private GameObject StarLevelUI;
    [SerializeField] private GameObject StarLevelPrefab;
    
    [SerializeField] private TextMeshProUGUI Hp;
    [SerializeField] private TextMeshProUGUI PhysicsAttack;
    [SerializeField] private TextMeshProUGUI MagicAttack;
    [SerializeField] private TextMeshProUGUI Speed;
    [SerializeField] private TextMeshProUGUI Critical;
    [SerializeField] private TextMeshProUGUI Cooldown;
    
    [SerializeField] private TextMeshProUGUI HpChange;
    [SerializeField] private TextMeshProUGUI PhysicsAttackChange;
    [SerializeField] private TextMeshProUGUI MagicAttackChange;
    [SerializeField] private TextMeshProUGUI CriticalChange;
    [SerializeField] private TextMeshProUGUI CooldownChange;

    public bool fadeUI;
    
    // A dictionary that stores the original transparency value
    private Dictionary<Image, float> originalImageAlphas = new Dictionary<Image, float>();
    private Dictionary<TMP_Text, float> originalTextMeshProAlphas = new Dictionary<TMP_Text, float>();

    public void Hide(){
        this.gameObject.SetActive(false);
    }

    private void Update() {
        SetAllUITransparency(this.gameObject,fadeUI,0.1f);
    }

    public void ChangeHeroDetailUIValue(Sprite sprite){
        heroImage.sprite = sprite;
    }

    public void ChangeDetailUI(Hero hero){
        Name.text = hero.Name;
        Description.text = hero.Description;
        UpdateStarLevelUI(hero);
        Hp.text = hero.InitialHealth.ToString();
        PhysicsAttack.text = hero.InitialPhysicsAttack.ToString();
        MagicAttack.text = hero.InitialMagicAttack.ToString();
        Speed.text = hero.Speed.ToString();
        Critical.text = hero.InitialCritical.ToString();
        if (hero.FighterSkillCaster) {
            Cooldown.text = hero.FighterSkillCaster.GetInitialData(SkillProperty.Cooldown).ToString();
        }
        UpdateDetailUI(hero);
    }

    private string GetPropertyDiff(float current, float initial){
        float diff = current - initial;
        if (diff == 0){
            return "";
        }
        string sign = diff > 0 ? "+" : "";
        return sign + diff;
    }

    public void UpdateDetailUI(Hero hero){
        HpChange.text = GetPropertyDiff(hero.Health, hero.InitialHealth);
        PhysicsAttackChange.text = GetPropertyDiff(hero.PhysicsAttack, hero.InitialPhysicsAttack);
        MagicAttackChange.text = GetPropertyDiff(hero.MagicAttack, hero.InitialMagicAttack);
        CriticalChange.text = GetPropertyDiff(hero.Critical, hero.InitialCritical);
        if (hero.FighterSkillCaster) {
            CooldownChange.text = GetPropertyDiff(hero.FighterSkillCaster.GetCurrentData(SkillProperty.Cooldown),
                hero.FighterSkillCaster.GetInitialData(SkillProperty.Cooldown));
        }
    }

    private void UpdateStarLevelUI(Hero hero){
        foreach (Transform child in StarLevelUI.transform){
            Destroy(child.gameObject);
        }

        for (int i = 0; i < hero.StarLevel; i++){
            GameObject go = Instantiate(StarLevelPrefab, StarLevelUI.transform);
        }
        
    }

    /// <summary>
    /// Sets the transparency of all Image components in the target object and its sub-objects
    /// </summary>
    private void SetImagesTransparency(GameObject target, bool isFaded, float fadedAlpha = 0.5f) {
        // 获取目标对象及其子对象中的所有Image组件
        Image[] images = target.GetComponentsInChildren<Image>(true);
        
        foreach (Image img in images)
        {
            // 跳过目标对象自身的Image组件
            if (img.transform == target.transform)
                continue;
            
            // 如果是变淡操作且尚未存储原始值
            if (isFaded && !originalImageAlphas.ContainsKey(img))
            {
                // 存储原始透明度
                originalImageAlphas.Add(img, img.color.a);
            }

            // 获取当前颜色
            Color newColor = img.color;
            
            if (isFaded)
            {
                // 设置为变淡透明度
                newColor.a = fadedAlpha;
            }
            else
            {
                // 恢复原始透明度（如果之前存储过）
                if (originalImageAlphas.TryGetValue(img, out float originalAlpha))
                {
                    newColor.a = originalAlpha;
                }
                else
                {
                    // 如果没有存储过原始值，默认恢复为完全不透明
                    newColor.a = 1f;
                }
            }
            
            // 应用新颜色
            img.color = newColor;
            
            // 如果取消变淡且已存储过该组件的原始值，从字典中移除
            if (!isFaded && originalImageAlphas.ContainsKey(img))
            {
                originalImageAlphas.Remove(img);
            }
        }
    }
    
    
    public void SetTextMeshProTransparency(GameObject target, bool isFaded, float fadedAlpha = 0.5f)
    {
        // 获取目标对象及其子对象中的所有TextMeshPro组件
        TMP_Text[] textMeshPros = target.GetComponentsInChildren<TMP_Text>(true);
        
        foreach (TMP_Text tmpText in textMeshPros)
        {
            // 跳过目标对象自身的TextMeshPro组件
            if (tmpText.transform == target.transform)
                continue;
            
            // 如果是变淡操作且尚未存储原始值
            if (isFaded && !originalTextMeshProAlphas.ContainsKey(tmpText))
            {
                // 存储原始透明度
                originalTextMeshProAlphas.Add(tmpText, tmpText.color.a);
            }

            // 获取当前颜色
            Color newColor = tmpText.color;
            
            if (isFaded)
            {
                // 设置为变淡透明度
                newColor.a = fadedAlpha;
            }
            else
            {
                // 恢复原始透明度（如果之前存储过）
                if (originalTextMeshProAlphas.TryGetValue(tmpText, out float originalAlpha))
                {
                    newColor.a = originalAlpha;
                }
                else
                {
                    // 如果没有存储过原始值，默认恢复为完全不透明
                    newColor.a = 1f;
                }
            }
            
            // 应用新颜色
            tmpText.color = newColor;
            
            // 如果取消变淡且已存储过该组件的原始值，从字典中移除
            if (!isFaded && originalTextMeshProAlphas.ContainsKey(tmpText))
            {
                originalTextMeshProAlphas.Remove(tmpText);
            }
        }
    }
    
    public void SetAllUITransparency(GameObject target, bool isFaded, float fadedAlpha = 0.5f)
    {
        SetImagesTransparency(target, isFaded, fadedAlpha);
        SetTextMeshProTransparency(target, isFaded, fadedAlpha);
    }

    /// <summary>
    /// 示例方法：用于UI复选框的点击事件
    /// </summary>
    /// <param name="toggle">Toggle组件传来的状态</param>
    public void OnToggleFade(Toggle toggle)
    {
        SetImagesTransparency(gameObject, toggle.isOn);
    }
    
    
    
    
}
