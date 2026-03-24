using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleManager : StateMachineController{

    public static BattleManager Instance;

    [SerializeField] private float BaseBondValue = 20.0f;
    
    [field: SerializeField] public Transform EnemyParent { get; private set; }
    [SerializeField] private Transform HeroParent;
    
    [SerializeField] private AudioClip EquipPassiveEntrySfx;
    [SerializeField] private AudioClip UndressPassiveEntrySfx;
    
    [Header("Deploy Place Settings")] 
    [SerializeField] private HeroDeployAreaData[] HeroDeployPlaceArea;
    private int[] DeployAreaCurrentHeroCount;
    
    private Dictionary<Hero, PassiveEntry> Skills1InBattle;
    private Dictionary<Hero, PassiveEntry> Skills2InBattle;

    public List<Hero> HeroesInBattle{ get; private set; }
    public List<Enemy> EnemiesInBattle { get; private set; } = new();

    public Action<Hero> OnHeroEnterTheField;
    public Action<Hero> OnHeroExitTheField;
    
    public Action OnBattleStart;
    public Action OnBattleStartInRound;

    public bool IsGameOver => EnemiesInBattle.Count <= 0 || HeroesInBattle.Count <= 0;
    public bool IsFullHero => this.HeroesInBattle.Count >= this.Data.MaxHeroCount;

    private Hero selectedHero;
    private PrepareState Prepare;
    private InBattleState InBattle;

    public bool IsBattleStart { get; private set; }
    public bool IsVictory => this.CurrentState is VictoryState;

#if TEST_BATTLE
    [field: SerializeField] public BattleData Data { get; private set; }
#else
    public BattleData Data{ get; private set; }
#endif

    public Action OnEnemyBeClear;
    public Action OnRewindBattle;

    public List<string> BeforeBattleHeroes { get; private set; } = new();

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
        InBattle = GetComponent<InBattleState>();
        if (InBattle is CorridorInBattleState corridorInBattle) {
            corridorInBattle.OnEnemyBeClear += () => OnEnemyBeClear?.Invoke();
        }

        Skills1InBattle = new Dictionary<Hero, PassiveEntry>();
        Skills2InBattle = new Dictionary<Hero, PassiveEntry>();

        if (this.HeroDeployPlaceArea != null && this.HeroDeployPlaceArea.Length != 0) {
            this.DeployAreaCurrentHeroCount = new int[this.HeroDeployPlaceArea.Length];    
        }

#if TEST_BATTLE
        if (this.EnemyParent.childCount != 0) {
            foreach (Transform child in this.EnemyParent.transform) {
                if (child.TryGetComponent(out Enemy enemy)) {
                    this.EnemiesInBattle.Add(enemy);
                }
            }
        }
#endif
    }

    private void Start(){
        
#if TEST_BATTLE
            HeroWarehouseManager.Instance.TEMPFORBATTLE();
            PassiveEntryWarehouseManager.Instance.TEMPFORBATTLE();
            EntanglementManager.Instance.TEMPFORBATTLE();
#endif
        
        ChangeState(Prepare);

        if (this.Data) {
            BattleUIManager.Instance.SetBattleMessage(this.Data.BattleName, this.Data.BattleMessage);
        }

        BattleUIManager.Instance.SetHeroWarehouseActive(true);
        BattleUIManager.Instance.SetHeroPortraitActive(false);
        
        // TODO: Optimize Framerate
        Application.targetFrameRate = 120;
        
#if TEST_BATTLE
        SetBattleData(this.Data);
#endif
    }

    public void SetBattleData(BattleData data){
        this.Data = data;
        DeployEnemy();
        DeployHero();
    }

    public void RewindBattle() {
        if (!this.IsBattleStart) return;
        this.enabled = false;
        StopAllCoroutines();
        StartCoroutine(RewindBattleCoroutine());
    }

    private IEnumerator RewindBattleCoroutine() {
        this.AllHeroRecall();
        foreach (Transform child in this.HeroParent) {
            Destroy(child.gameObject);
        }

        yield return null;
        OnRewindBattle?.Invoke();
        GameManager.Instance.GoToBattle(this.Data, false, GameManager.Instance.IsTrainBattle);
    }

    private void DeployEnemy(){
        if (!this.Data) return;
        List<EnemyDepartmentData> departmentAreaData = this.Data.EnemiesInBattle;
        foreach (EnemyDepartmentData data in departmentAreaData){
            Enemy enemy = Instantiate(data.EnemyPrefab, this.EnemyParent);
            GetNavMeshPosition(data.Position, 1.0f, out Vector3 finalPos);
            enemy.Deploy(finalPos);
            this.EnemiesInBattle.Add(enemy);
        }
    }

    private void DeployHero() {
        if (!this.Data) return;
        List<HeroDepartmentArea> departmentAreaData = this.Data.HeroesInBattle;
        foreach (HeroDepartmentArea data in departmentAreaData){
            Hero hero = Instantiate(data.HeroPrefab, this.HeroParent);
            hero.SetOriginExist();
            GetNavMeshPosition(data.Position, 1.0f, out Vector3 finalPos);
            int deployAreaIndex = this.IsWithinArea(finalPos);
            if (deployAreaIndex != -1){
                hero.transform.position = finalPos;
                DraggableUI dragHero = hero.AddComponent<DraggableUI>();
                dragHero.prefabReference = hero.Name;
                hero.Deploy(deployAreaIndex);
                if (data.IsDeadBattleDefeat) {
                    hero.AddComponent<FighterDeadBattleDefeat>();
                }
            }else{
                Destroy(hero.gameObject);
            }
        }
    }

    public void AddEnemiesInParent(Transform parent) {
        foreach (Transform child in parent.transform) {
            if (child.TryGetComponent(out Enemy enemy)) {
                this.EnemiesInBattle.Add(enemy);
            }
        }
    }

    public void StartBattleInRound() {
        foreach (Enemy enemy in this.EnemiesInBattle){
            enemy.BattleStart();
        }
        foreach (Hero hero in this.HeroesInBattle) {
            hero.BattleStart();
        }
        OnBattleStartInRound?.Invoke();
    }

    public void StartBattle(){
        if (this.HeroesInBattle.Count <= 0){
            AudioManager.Instance.PlayErrorSfx();
            return;
        }

        IsBattleStart = true;
        foreach (HeroDeployAreaData area in this.HeroDeployPlaceArea) {
            area.DeployArea.gameObject.SetActive(false);    
        }
        
        foreach (Hero hero in this.HeroesInBattle) {
            this.BeforeBattleHeroes.Add(hero.Name);
        }

#if DEBUG_MODE
        this.BattleStartTime = Time.time;
#endif
        BattleUIManager.Instance.SetHeroPortraitActive(true);
        BattleUIManager.Instance.heroPortraitUI.CreateUIProtraits(HeroesInBattle);
        ChangeState(InBattle);
        OnBattleStart?.Invoke();
        SaveHeroDeploy();
        SaveHeroPassiveEntry();
    }

    private void SaveHeroDeploy() {
        List<HeroDeployData> data = new List<HeroDeployData>();
        foreach (Hero hero in HeroesInBattle) {
            if(hero.IsOriginExist) continue;
            data.Add(new HeroDeployData {
                HeroName = hero.Name,
                HeroPosition = hero.transform.position,
            });
        }
        HeroDeploySaveData saveData = new HeroDeploySaveData { Datas = data };
        string json = JsonUtility.ToJson(saveData);
        if (PlayerPrefs.HasKey(this.Data.BattleName)) {
            PlayerPrefs.DeleteKey(this.Data.BattleName);
        }
        PlayerPrefs.SetString(this.Data.BattleName, json);
    }

    public void LoadHeroDeploy() {
        if (!this.Data) return;
        if (!PlayerPrefs.HasKey(this.Data.BattleName)) return;
        HeroDeploySaveData saveData = JsonUtility.FromJson<HeroDeploySaveData>(PlayerPrefs.GetString(this.Data.BattleName));
        foreach (HeroDeployData data in saveData.Datas) {
            if (IsFullHero) break;
            foreach (Transform child in BattleUIManager.Instance.HeroWarehouseParent) {
                if (child.TryGetComponent(out DraggableUI draggable)
                    && draggable.prefabReference == data.HeroName) {
                    draggable.DeployHero(data.HeroPosition);
                    break;
                }
            }
        }
    }

    private void SaveHeroPassiveEntry() {
        // TODO: Optimize Speed
        foreach (Hero hero in this.HeroesInBattle) {
            List<PassiveEntry> passiveEntries = hero.GetHeroPassiveEntries();
            string key = hero.Name + "PassiveEntry";
            if (PlayerPrefs.HasKey(key)) {
                PlayerPrefs.DeleteKey(key);
            }
            if (passiveEntries == null || passiveEntries.Count == 0) {
                continue;
            }
            string saveContent = passiveEntries[0].Data.Name;
            for (int i = 1; i < passiveEntries.Count; i++) {
                saveContent += "|" + passiveEntries[i].Data.Name;
            }
            PlayerPrefs.SetString(key, saveContent);
        }
    }

    public void LoadHeroPassiveEntry(Hero hero) {
        // TODO: Optimize Speed
        if (!PlayerPrefs.HasKey(hero.Name + "PassiveEntry")) return;
        string result = PlayerPrefs.GetString(hero.Name + "PassiveEntry", "");
        if (result == "") {
            return;
        }

        this.selectedHero = hero;
        string[] passiveEntries = result.Split("|");
        foreach (string entryName in passiveEntries) {
            PassiveEntry data = PassiveEntryWarehouseManager.Instance.GetPassiveEntryByName(entryName);
            if (data) {
                this.AddPassiveEntry(data);
            }
        }
    }

    public void BattleVictoryAddHeroBond() {
        for (int i = 0; i < this.BeforeBattleHeroes.Count; i++) {
            for (int j = i + 1; j < this.BeforeBattleHeroes.Count; j++) {
                EntanglementManager.Instance.AddEntanglementValue(this.BeforeBattleHeroes[i],
                    this.BeforeBattleHeroes[j], this.BaseBondValue * this.Data.BondMultiplier);
            }
        }
    }

    public void AllHeroRecall(bool isForceOriginExistHero = false) {
        for (int i = 0; i < HeroesInBattle.Count; ) {
            this.selectedHero = HeroesInBattle[i];
            if (!isForceOriginExistHero && (!this.selectedHero || this.selectedHero.IsOriginExist)) {
                i++;
                continue;
            }
            RecallSelectedHero();
        }
    }

    public void BattleVictory() {
        if (TryGetComponent(out VictoryState victory)) {
            ChangeState(victory);    
        }
    }

    public void BattleDefeat() {
        if (TryGetComponent(out DefeatState defeat)) {
            ChangeState(defeat);    
        }
    }

    public void AddHero(Hero hero){
        hero.transform.parent = this.HeroParent;
        this.DeployAreaCurrentHeroCount[hero.DeployAreaIndex]++;
        OnHeroEnterTheField?.Invoke(hero);
        HeroesInBattle.Add(hero);
    }

    public int IsWithinArea(Vector3 targetPos) {
        for (int i = 0; i < HeroDeployPlaceArea.Length; i++) {
            HeroDeployAreaData areaData = HeroDeployPlaceArea[i];
            if (areaData.DeployArea.bounds.Contains(targetPos) && this.DeployAreaCurrentHeroCount[i] < areaData.MaxHeroCount) {
                return i;
            }
        }
        return -1;
    }
    
    protected override void Update(){
        base.Update();
        if (Input.GetMouseButtonDown(0)){
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            Hero hero = null;
            foreach (var res in results) {
                if (res.gameObject.layer == LayerMask.NameToLayer("UI")) {
                    return ;
                }

                if (res.gameObject.layer == LayerMask.NameToLayer("Hero")) {
                    res.gameObject.TryGetComponent(out hero);
                }
            }
            SelectObject(hero);
        }
    }
    
    private void SelectObject(Hero so){
        if (this.CurrentState is not PrepareState) return;
        selectedHero = so;
        if(!selectedHero){
            BattleUIManager.Instance.HideHeroDetail();
        }else{
            ShowHeroDetail(selectedHero);
        }
    }

    public void ShowHeroDetail(Hero hero){
        this.selectedHero = hero;
        hero.UpdateByFighterTypeCountPropertyChange();
        UpdatePassiveEntryUI(hero);
        BattleUIManager.Instance.ShowHeroDetail(hero);
    }

    public void RecallHero(Hero hero, bool isDestroy = true) {
        BattleUIManager.Instance.HideHeroDetail();
        selectedHero = hero;
        this.RecallSelectedHero(isDestroy);
    }

    /// <summary>
    /// 召回英雄
    /// </summary>
    public void RecallSelectedHero(bool isDestroy = true) {
        if (!selectedHero) return;
        this.RemoveHero(selectedHero);
        if (isDestroy) {
            BattleUIManager.Instance.heroWarehouseUI.AddItem(selectedHero.Name);
            Destroy(selectedHero.gameObject);
        }
        selectedHero = null;
        BattleUIManager.Instance.HideHeroDetail();
    }
    
    /// <summary>
    /// 添加技能到空槽位，成功返回true，失败返回false。
    /// </summary>
    public int AddPassiveEntry(PassiveEntry data){
        if (!selectedHero) return -1;
        if (!data.Precondition(selectedHero)) return -1;
        if (!PassiveEntryWarehouseManager.Instance.ContainsPassiveEntry(data.Data.Name)) {
            return -1;
        }

        if (Skills1InBattle.TryAdd(selectedHero, data)){
            selectedHero.AddPassiveEntry(data, false);
            BattleUIManager.Instance.heroDetailUI.UpdateDetailUI(selectedHero);
            UpdatePassiveEntryUI(selectedHero);
            PassiveEntryWarehouseManager.Instance.RemovePassiveEntry(data.Data.Name);
            if (EquipPassiveEntrySfx) {
                AudioManager.Instance.PlaySfxAtPoint(this.transform.position, EquipPassiveEntrySfx);
            }
            return 0;
        }
        
        if (Skills2InBattle.TryAdd(selectedHero, data)){
            selectedHero.AddPassiveEntry(data, false);
            BattleUIManager.Instance.heroDetailUI.UpdateDetailUI(selectedHero);
            UpdatePassiveEntryUI(selectedHero);
            PassiveEntryWarehouseManager.Instance.RemovePassiveEntry(data.Data.Name);
            if (EquipPassiveEntrySfx) {
                AudioManager.Instance.PlaySfxAtPoint(this.transform.position, EquipPassiveEntrySfx);
            }
            return 1;
        }
        SceneChangeManager.Instance.AddGameTip("芯片已满！");
        AudioManager.Instance.PlayErrorSfx();
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
        if (UndressPassiveEntrySfx && !IsBattleStart) {
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, UndressPassiveEntrySfx);
        }
        if (Skills1InBattle.Remove(selectedHero, out PassiveEntry removedSkillData)){
            RecallSelectedPassiveEntry(removedSkillData);
            selectedHero.RemovePassiveEntry(removedSkillData, false);

            if (!IsBattleStart) {
                BattleUIManager.Instance.heroDetailUI.UpdateDetailUI(selectedHero);
                UpdatePassiveEntryUI(selectedHero);
            }
        }
    }
    
    /// <summary>
    /// 只从第二个技能槽中移除指定 GameObject 的技能。
    /// </summary>
    public void RemoveSkillFromSlot2(){
        if (UndressPassiveEntrySfx&& !IsBattleStart) {
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, UndressPassiveEntrySfx);
        }
        if (Skills2InBattle.Remove(selectedHero, out PassiveEntry removedSkillData)){
            RecallSelectedPassiveEntry(removedSkillData);
            selectedHero.RemovePassiveEntry(removedSkillData, false);
            
            if (!IsBattleStart) {
                BattleUIManager.Instance.heroDetailUI.UpdateDetailUI(selectedHero);
                UpdatePassiveEntryUI(selectedHero);
            }
        }
    }

    private void RecallSelectedPassiveEntry(PassiveEntry passiveEntry, int count = 1){
        PassiveEntryWarehouseManager.Instance.AddPassiveEntry(passiveEntry.Data.Name, count);
        BattleUIManager.Instance.PassiveEntryWarehouseUI.RecallPassiveEntry(passiveEntry, count);
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
    

    public void RemoveHero(Hero hero) {
        this.selectedHero = hero;
        this.DeployAreaCurrentHeroCount[hero.DeployAreaIndex]--;
        OnHeroExitTheField?.Invoke(hero);
        this.HeroesInBattle.Remove(hero);
        RemovePassiveEntry();
        hero.UndressSelfEntry();
    }

    public void RemoveEnemy(Enemy enemy) {
        this.EnemiesInBattle.Remove(enemy);
    }

    public bool HasBeDamagedTarget(TargetType type) {
        if (type == TargetType.Hero){
            foreach (Hero hero in HeroesInBattle){
                if (hero.HealthPercentage < 1.0f) {
                    return true;
                }
            }    
        }else if (type == TargetType.Enemy){
            foreach (Enemy enemy in EnemiesInBattle){
                if (enemy.HealthPercentage < 1.0f) {
                    return true;
                }
            }    
        }
        return false;
    }

    public Fighter FindMinPercentagePropertyHero(FighterProperty property, TargetType type){
        Fighter result = null;
        float minPercentage = 1.0f;
        if (type == TargetType.Hero){
            foreach (Hero hero in HeroesInBattle){
                if (hero.HealthPercentage < minPercentage){
                    minPercentage = hero.HealthPercentage;
                    result = hero;
                }
            }    
        }else if (type == TargetType.Enemy){
            foreach (Enemy enemy in EnemiesInBattle){
                if (enemy.HealthPercentage < minPercentage){
                    minPercentage = enemy.HealthPercentage;
                    result = enemy;
                }
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

    public Fighter FindFurthestHeroTarget(Vector3 position) {
        float maxDistance = -1.0f;
        Fighter result = null;
        foreach (Hero hero in HeroesInBattle) {
            float distance = (position - hero.transform.position).sqrMagnitude;
            if (distance > maxDistance) {
                maxDistance = distance;
                result = hero;
            }
        }
        return result;
    }

    private List<Fighter> GetSortedFightersByDistance(Fighter self) {
        List<Fighter> result = new List<Fighter>();
        if (self.AttackTargetType == TargetType.Hero) {
            foreach (Hero hero in this.HeroesInBattle) {
                result.Add(hero);
            }
        }else if (self.AttackTargetType == TargetType.Enemy) {
            foreach (Enemy enemy in this.EnemiesInBattle) {
                result.Add(enemy);
            }
        }
        result.Sort((Fighter f1, Fighter f2) => {
            float d1 = (self.transform.position - f1.transform.position).sqrMagnitude;
            float d2 = (self.transform.position - f2.transform.position).sqrMagnitude;
            return d1 > d2 ? 1 : (d1 < d2 ? -1 : 0);
        });
        return result;
    }

    public Fighter GetNearestFighter(Fighter selfFighter, Func<Fighter, bool> condition = null) {
        List<Fighter> sortedFighter = GetSortedFightersByDistance(selfFighter);
        if (condition == null) return sortedFighter[0];
        foreach (Fighter f in sortedFighter) {
            if (condition(f)) {
                return f;
            }
        }
        return null;
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

