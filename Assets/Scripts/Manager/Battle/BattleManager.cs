using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleManager : StateMachineController{

    public static BattleManager Instance;

    [SerializeField] private Transform EnemyParent;
    [SerializeField] private Transform HeroParent;

    [SerializeField] private AudioClip ErrorSfx;
    [SerializeField] private AudioClip EquipPassiveEntrySfx;
    [SerializeField] private AudioClip UndressPassiveEntrySfx;

    // TODO: Get BattleData From World Scene
    [SerializeField] private BattleData Data;
    [SerializeField] private TextMeshProUGUI BattleNameText;
    [SerializeField] private TextMeshProUGUI BattleMessageText;

    [Header("Deploy Place Settings")] [SerializeField]
    private BoxCollider HeroDeployPlaceArea;

    private Dictionary<Hero, PassiveEntry> Skills1InBattle;
    private Dictionary<Hero, PassiveEntry> Skills2InBattle;

    public List<Hero> HeroesInBattle{ get; private set; }
    public List<Enemy> EnemiesInBattle{ get; private set; }

    public Action<Hero> OnHeroEnterTheField;
    public Action<Hero> OnHeroExitTheField;

    public bool IsGameOver => EnemiesInBattle.Count <= 0 || HeroesInBattle.Count <= 0;
    public bool IsFullHero => this.HeroesInBattle.Count >= this.Data.MaxHeroCount;

    private Hero selectedHero;
    private PrepareState Prepare;

#if DEBUG_MODE
    public float BattleStartTime {get; private set;}
#endif

    private void Awake(){
        if (Instance != null){
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        HeroesInBattle = new List<Hero>();
        Prepare = GetComponent<PrepareState>();
        Skills1InBattle = new Dictionary<Hero, PassiveEntry>();
        Skills2InBattle = new Dictionary<Hero, PassiveEntry>();
        DeployEnemy();
    }

    private void Start(){
        ChangeState(Prepare);
        this.BattleNameText.text = this.Data.BattleName;
        this.BattleMessageText.text = this.Data.BattleMessage;
        BattleUIManager.Instance.SetHeroWarehouseActive(true);
        BattleUIManager.Instance.SetHeroPanelActive(false);
        BattleUIManager.Instance.SetHeroPortraitActive(false);
    }

    public void PlayErrorSfx(){
        if(ErrorSfx) AudioManager.Instance.PlaySfxAtPoint(this.transform.position, ErrorSfx);
    }

    private void DeployEnemy(){
        this.EnemiesInBattle = new List<Enemy>();
        if (!this.Data) return;
        List<EnemyDepartmentData> departmentAreaData = this.Data.EnemiesInBattle;
        foreach (EnemyDepartmentData data in departmentAreaData){
            Enemy enemy = Instantiate(data.EnemyPrefab, this.EnemyParent);
            GetNavMeshPosition(data.Position, 1.0f, out Vector3 finalPos);
            enemy.transform.position = finalPos;
            this.EnemiesInBattle.Add(enemy);
        }
    }

    public void StartBattle(){
        if (this.HeroesInBattle.Count <= 0){
            PlayErrorSfx();
            return;
        }
        this.HeroDeployPlaceArea.gameObject.SetActive(false);
        ChangeState(GetComponent<InBattleState>());
#if DEBUG_MODE
        this.BattleStartTime = Time.time;
#endif
        BattleUIManager.Instance.SetHeroPortraitActive(true);
        BattleUIManager.Instance.heroPortraitUI.PushHeros(HeroesInBattle);
    }

    public void AddHero(Hero hero){
        hero.transform.parent = this.HeroParent;
        OnHeroEnterTheField?.Invoke(hero);
        HeroesInBattle.Add(hero);
    }
    
    public bool IsWithinArea(Vector3 targetPos){
        return this.HeroDeployPlaceArea.bounds.Contains(targetPos);
    }
    
    protected override void Update(){
        base.Update();
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0)){
            Ray ray = CameraManager.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit,maxDistance:100f,layerMask:LayerMask.GetMask("Hero"))
                && hit.collider.gameObject.TryGetComponent(out Hero hero)){
                SelectObject(hero);
            }else{
                SelectObject(null);
            }
        }
        
        
    }
    
    private void SelectObject(Hero so){
        if (this.CurrentState is not PrepareState) return;
        selectedHero = so;
        if(!selectedHero){
            BattleUIManager.Instance.SetHeroPanelActive(false);
        }else{
            ShowHeroDetail(selectedHero);
        }
    }

    public void ShowHeroDetail(Hero hero){
        BattleUIManager.Instance.SetHeroPanelActive(true);
        UpdatePassiveEntryUI(hero);
        BattleUIManager.Instance.heroDetailUI.ChangeHeroDetailUIValue(hero.StandingSprite);
        BattleUIManager.Instance.heroDetailUI.ChangeDetailUI(hero);
        BattleUIManager.Instance.UpdateSelectedHeroSkillUI(hero.Type,
            hero.FighterSkillCaster.Data.Description,
            hero.GetPassiveEntryDesc());
    }

    /// <summary>
    /// 召回英雄
    /// </summary>
    public void RecallSelectedHero(){
        RemovePassiveEntry();
        BattleUIManager.Instance.heroWarehouseUI.AddItem(selectedHero.name);
        this.RemoveHero(selectedHero);
        Destroy(selectedHero.gameObject);
        selectedHero = null;
        BattleUIManager.Instance.SetHeroPanelActive(false);
    }
    
    /// <summary>
    /// 添加技能到空槽位，成功返回true，失败返回false。
    /// </summary>
    public int AddPassiveEntry(PassiveEntry data){
        if (!selectedHero) return -1;
        if (!data.Precondition(selectedHero)) return -1;

        if (EquipPassiveEntrySfx) {
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, EquipPassiveEntrySfx);
        }

        if (Skills1InBattle.TryAdd(selectedHero, data)){
            selectedHero.AddPassiveEntry(data);
            BattleUIManager.Instance.heroDetailUI.UpdateDetailUI(selectedHero);
            UpdatePassiveEntryUI(selectedHero);
            return 0;
        }
        
        if (Skills2InBattle.TryAdd(selectedHero, data)){
            selectedHero.AddPassiveEntry(data);
            BattleUIManager.Instance.heroDetailUI.UpdateDetailUI(selectedHero);
            UpdatePassiveEntryUI(selectedHero);
            return 1;
        }

        return -1;
    }
    
    /// <summary>
    /// 清除指定 GameObject 的技能。
    /// </summary>
    private void RemovePassiveEntry(){
        RemoveSkillFromSlot1();
        RemoveSkillFromSlot2();
    }
    
    /// <summary>
    /// 只从第一个技能槽中移除指定 GameObject 的技能。
    /// </summary>
    public void RemoveSkillFromSlot1(){
        if (UndressPassiveEntrySfx) {
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, UndressPassiveEntrySfx);
        }
        if (Skills1InBattle.Remove(selectedHero, out PassiveEntry removedSkillData)){
            selectedHero.RemovePassiveEntry(removedSkillData);
            BattleUIManager.Instance.heroDetailUI.UpdateDetailUI(selectedHero);
            RecallSelectedPassiveEntry(removedSkillData);
            UpdatePassiveEntryUI(selectedHero);
        }
    }
    
    /// <summary>
    /// 只从第二个技能槽中移除指定 GameObject 的技能。
    /// </summary>
    public void RemoveSkillFromSlot2(){
        if (UndressPassiveEntrySfx) {
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, UndressPassiveEntrySfx);
        }
        if (Skills2InBattle.Remove(selectedHero, out PassiveEntry removedSkillData)){
            selectedHero.RemovePassiveEntry(removedSkillData);
            BattleUIManager.Instance.heroDetailUI.UpdateDetailUI(selectedHero);
            RecallSelectedPassiveEntry(removedSkillData);
            UpdatePassiveEntryUI(selectedHero);
        }
    }

    private void RecallSelectedPassiveEntry(PassiveEntry passiveEntry){
        BattleUIManager.Instance.PassiveEntryWarehouseUI.AddItem(passiveEntry);
    }
    
    /// <summary>
    /// 根据 selectedHero 查找其两个技能，并更新 skill1UI 和 skill2UI 上的文本
    /// </summary>
    private void UpdatePassiveEntryUI(Hero hero){
        string skill1Description = Skills1InBattle.TryGetValue(hero, out PassiveEntry skill1) ? skill1.Data.Description : "";
        string skill2Description = Skills2InBattle.TryGetValue(hero, out PassiveEntry skill2) ? skill2.Data.Description : "";
        BattleUIManager.Instance.SetSkill1UIText(skill1Description);
        BattleUIManager.Instance.SetSkill2UIText(skill2Description);
    }
    

    public void RemoveHero(Hero hero){
        OnHeroExitTheField?.Invoke(hero);
        this.HeroesInBattle.Remove(hero);
        hero.Undress();
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

    public Fighter FindFurthestEnemyTarget(Vector3 position) {
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
    
    public Fighter GetRandomFighter(TargetType type, Func<Fighter, bool> condition = null) {
        if (IsGameOver) return null;
        int randomIndex = -1;
        switch (type) {
            case TargetType.Hero:
                randomIndex = UnityEngine.Random.Range(0, this.HeroesInBattle.Count);
                break;
            case TargetType.Enemy:
                randomIndex = UnityEngine.Random.Range(0, this.EnemiesInBattle.Count);
                break;
        }

        if (randomIndex == -1) return null;
        if (condition == null)
            return type == TargetType.Hero ? this.HeroesInBattle[randomIndex] : this.EnemiesInBattle[randomIndex];

        Fighter fighter = type == TargetType.Hero ? this.HeroesInBattle[randomIndex] : this.EnemiesInBattle[randomIndex];
        if (condition(fighter)) return fighter;
        int index = randomIndex + 1;
        fighter = type == TargetType.Hero ? this.HeroesInBattle[index % this.HeroesInBattle.Count] 
            : this.EnemiesInBattle[index % this.EnemiesInBattle.Count];
            
        while (index % this.HeroesInBattle.Count != randomIndex && !condition(fighter)){
            index++;
            fighter = type == TargetType.Hero ? this.HeroesInBattle[index % this.HeroesInBattle.Count] 
                : this.EnemiesInBattle[index % this.EnemiesInBattle.Count];
        }

        if (index % this.HeroesInBattle.Count == randomIndex) return null;
        return fighter;
    }
    
    private void GetNavMeshPosition(Vector3 currentPos, float maxDistance, out Vector3 navMeshPos){
        if (UnityEngine.AI.NavMesh.SamplePosition(currentPos, out var hit, maxDistance, UnityEngine.AI.NavMesh.AllAreas)){
            navMeshPos = hit.position;
            return;
        }
        navMeshPos = currentPos;
    }

}

