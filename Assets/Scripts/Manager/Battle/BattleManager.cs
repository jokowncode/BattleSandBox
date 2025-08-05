using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleManager : StateMachineController {

    public static BattleManager Instance;

    [SerializeField] private AudioClip EquipPassiveEntrySfx;
    [SerializeField] private AudioClip UndressPassiveEntrySfx;
    
    // TODO: Get BattleData From World Scene
    [SerializeField] private BattleData Data;
    [SerializeField] private EnemyDepartmentArea EnemyArea;

    [Header("Deploy Place Settings")]
    [SerializeField] private GameObject deployPlace;
    public int width;
    public int height;
   
    
    public Dictionary<Hero,SkillData> Skills1InBattle;
    public Dictionary<Hero,SkillData> Skills2InBattle;

    public List<Hero> HeroesInBattle{ get; private set; }
    [field: SerializeField] public List<Enemy> EnemiesInBattle { get; private set; }

    // TODO: eg:Support Passive Entry Register Action to Change Hero Property
    public Action<Hero> OnHeroEnterTheField;
    public Action<Hero> OnHeroExitTheField;
    
    public bool IsGameOver => EnemiesInBattle.Count <= 0 || HeroesInBattle.Count <= 0;
    
    [Header("Selected Hero")]
    public static Hero selectedHero;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        HeroesInBattle = new List<Hero>();
        Skills1InBattle = new Dictionary<Hero,SkillData>();
        Skills2InBattle = new Dictionary<Hero,SkillData>();
        
        // TODO: Initiate Enemy
        // EnemiesInBattle = EnemyArea.InitializeEnemy(Data.EnemiesInBattle);
        this.HeroesInBattle = new List<Hero>();
        
        // Turn To Prepare State
        ChangeState(GetComponent<PrepareState>());
    }
    
    private void Start() {
        BattleUIManager.Instance.SetHeroWarehouseActive(true);
        BattleUIManager.Instance.SetHeroPanelActive(true);
        BattleUIManager.Instance.SetHeroPanelActive(false);
    }

    public void StartBattle(){
        ChangeState(GetComponent<InBattleState>());
    }
    
    public void AddHero(Hero hero){
        OnHeroEnterTheField?.Invoke(hero);
        this.HeroesInBattle.Add(hero);
    }
    
    public bool IsWithinArea(Vector3 targetPos){
        Vector3 checkPos = deployPlace.transform.position;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        // 判断 checkPos 是否在 targetPos 所定义的矩形范围内
        bool withinX = checkPos.x >= targetPos.x - halfWidth && checkPos.x <= targetPos.x + halfWidth;
        bool withinZ = checkPos.z >= targetPos.z - halfHeight && checkPos.z <= targetPos.z + halfHeight;

        return withinX && withinZ;
    }
    
    private void Update() {
        // 忽略点击UI的情况
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit,maxDistance:100f,layerMask:LayerMask.GetMask("Hero"))
                && hit.collider.gameObject.TryGetComponent(out Hero hero)){
                SelectObject(hero);
            }else{
                SelectObject(null);
            }
        }
    }
    
    public void SelectObject(Hero so) {
        selectedHero = so;
        if(!selectedHero){
            BattleUIManager.Instance.SetHeroPanelActive(false);
        }else{
            BattleUIManager.Instance.SetHeroPanelActive(true);
            UpdateSkillUI();
            BattleUIManager.Instance.heroDetailUI.ChangeHeroDetailUIValue(selectedHero.HeroRenderer.sprite);
            BattleUIManager.Instance.heroDetailUI.ChangeDetailUI(selectedHero);
            if (so.TryGetComponent(out Hero hero)) {
                BattleUIManager.Instance.UpdateSelectedHeroSkillUI(hero.Type,hero.FighterSkillCaster.Data.Description);
            }
        }
    }

    /// <summary>
    /// 召回英雄
    /// </summary>
    public void RecallSelectedHero() {
        RemoveSkill();
        BattleUIManager.Instance.heroWarehouseUI.AddItem(selectedHero.GetComponentInChildren<SpriteRenderer>().sprite,selectedHero.name);
        HeroesInBattle.Remove(selectedHero.GetComponent<Hero>());
        Destroy(selectedHero);
        BattleUIManager.Instance.SetHeroPanelActive(false);
    }
    
    /// <summary>
    /// 添加技能到空槽位，成功返回true，失败返回false。
    /// </summary>
    public int AddSkill(SkillData data){
        
        if(EquipPassiveEntrySfx)
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, EquipPassiveEntrySfx);
        
        if (Skills1InBattle.TryAdd(selectedHero, data)){
            UpdateSkillUI();
            Debug.Log("添加到 Skills1InBattle");
            return 0;
        }
        
        if (Skills2InBattle.TryAdd(selectedHero, data)){
            UpdateSkillUI();
            Debug.Log("添加到 Skills2InBattle");
            return 1;
        }
        return -1;
    }

    /// <summary>
    /// 清除指定 GameObject 的技能。
    /// </summary>
    private void RemoveSkill(){
        if (UndressPassiveEntrySfx)
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, UndressPassiveEntrySfx);
        RemoveSkillFromSlot1();
        RemoveSkillFromSlot2();
    }
    
    /// <summary>
    /// 只从第一个技能槽中移除指定 GameObject 的技能。
    /// </summary>
    public void RemoveSkillFromSlot1(){
        if (Skills1InBattle.Remove(selectedHero, out SkillData removedSkillData)){
            RecallSelectedSkill(removedSkillData);
            UpdateSkillUI();
        }

    }
    
    /// <summary>
    /// 只从第二个技能槽中移除指定 GameObject 的技能。
    /// </summary>
    public void RemoveSkillFromSlot2(){
        if (Skills2InBattle.Remove(selectedHero, out SkillData removedSkillData)){
            RecallSelectedSkill(removedSkillData);
            UpdateSkillUI();
        }
    }

    private void RecallSelectedSkill(SkillData skill){
        BattleUIManager.Instance.skillWarehouseUI.AddItem(skill);
    }
    
    /// <summary>
    /// 根据 selectedHero 查找其两个技能，并更新 skill1UI 和 skill2UI 上的文本
    /// </summary>
    private void UpdateSkillUI(){
        string skill1Description = "";
        string skill2Description = "";

        if (Skills1InBattle.TryGetValue(selectedHero, out SkillData skill1)){
            skill1Description = skill1.Description;
        }else{
            skill1Description = "";
        }

        if (Skills2InBattle.TryGetValue(selectedHero, out SkillData skill2)){
            skill2Description = skill2.Description;
        }else{
            skill2Description = "";
        }

        BattleUIManager.Instance.SetSkill1UIText(skill1Description);
        BattleUIManager.Instance.SetSkill2UIText(skill2Description);
    }
    
    public void RemoveHero(Hero hero){
        this.OnHeroExitTheField?.Invoke(hero);
        this.HeroesInBattle.Remove(hero);
    }

    public void RemoveEnemy(Enemy enemy) {
        this.EnemiesInBattle.Remove(enemy);
    }

    public Fighter FindMinPercentagePropertyHero(FighterProperty property){
        Fighter result = null;
        float minPercentage = 1.0f;
        foreach (Hero hero in HeroesInBattle){
            float currentValue = ReflectionTools.GetObjectProperty<float>(property.ToString(), hero);
            float initialValue = ReflectionTools.GetObjectProperty<float>("Initial"+property, hero);
            float percentage = currentValue / initialValue;
            if (percentage < minPercentage){
                minPercentage = percentage;
                result = hero;
            }
        }
        return result;
    }

    public Fighter FindFurthestTarget(Vector3 position) {
        float maxDistance = -1.0f;
        Fighter result = null;
        foreach (Enemy enemy in EnemiesInBattle) {
            float distance = (position - enemy.transform.position).sqrMagnitude;
            if (distance > maxDistance) {
                maxDistance = distance;
                result = enemy;
            }
        }
        return result;
    }
    
    public Fighter GetRandomFighter(TargetType type) {
        if (IsGameOver) return null;
        switch (type) {
            case TargetType.Hero:
                return this.HeroesInBattle[UnityEngine.Random.Range(0, this.HeroesInBattle.Count)];
            case TargetType.Enemy:
                return this.EnemiesInBattle[UnityEngine.Random.Range(0, this.EnemiesInBattle.Count)];
        }
        return null;
    }

}

