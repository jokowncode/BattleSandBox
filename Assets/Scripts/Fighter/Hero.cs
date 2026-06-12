using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class Hero : Fighter {

    [field: SerializeField] public HeroWarehouseData WarehouseData { get; private set; }
    [SerializeField] private PassiveEntry[] HeroSelfPassiveEntries;
    [field: SerializeField] public SkillCaster HeroUpdateSkillCaster { get; private set; }

    [SerializeField] private GameObject HeroDeploy;
    
    [Header("Revenge")] 
    [SerializeField] private BuffData RevengeBuff;
    
    private List<PassiveEntry> EquipPassiveEntries;
    private List<PassiveEntry> SelfPassiveEntries;
    
    // public SpriteRenderer HeroRenderer{ get; private set; }
    public int HeroAvailablePassiveEntrySortCode { get; private set; }
    public int DeployAreaIndex { get; private set; }
    public Vector3 StartPosition { get; private set; }

    public Dictionary<string, Object> Records = new();

    // public int MergeGroupIndex { get; set; } = -1;
    public bool IsOriginExist { get; set; } = false;

    public Dictionary<Hero, int> ShareDamageHeroes {get; private set;} = new();
    
    public bool IsMerge { get; set; } = false;

    protected override void Awake(){
        base.Awake();
        EquipPassiveEntries = new List<PassiveEntry>();
        SelfPassiveEntries = new List<PassiveEntry>();
        // HeroRenderer = GetComponentInChildren<SpriteRenderer>();

        if (this.FighterSkillCaster) {
            this.HeroAvailablePassiveEntrySortCode =
                (int)PassiveEntrySort.General | (int)PassiveEntrySort.Talent | (int)this.FighterSkillCaster.Sort;
        }
    }
    
    private void OnDestroy() {
        if (!IsSummon) {
            SaveDataManager.Instance.SetHeroHealth(this.Name, Mathf.Max(this.InBattleHealth, 0.0f));
        }
    }

    public void SetOriginExist() {
        this.IsOriginExist = true;
        this.InitHealth();
    }

    public void ShareDamage(Hero hero) {
        if (hero == this) return ;
        if (!this.ShareDamageHeroes.TryAdd(hero)) {
            this.ShareDamageHeroes[hero] += 1;
        }
    }

    public void RemoveShareDamage(Hero hero) {
        if (!hero || !this.ShareDamageHeroes.Contains(hero)) return ;
        this.ShareDamageHeroes[hero] -= 1;
        if (this.ShareDamageHeroes[hero] <= 0) {
            this.ShareDamageHeroes.Remove(hero);    
        }
    }

    public void StartRevengeVow(Hero hero) {
        hero.OnDead += OnRevengeDead;
    }
    
    public void StopRevengeVow(Hero hero) {
        if (!hero) return;
        hero.OnDead -= OnRevengeDead;
    }

    private void OnRevengeDead(Fighter deadHero) {
        deadHero.OnDead -= OnRevengeDead;
        BuffManager.Instance.AddBuff(this, this, this.RevengeBuff);
    }

    public void SetMergeData(FighterData data) {
        this.InitialData = data;
        this.CurrentData = Instantiate(this.InitialData);
    }

    public void SetMergeSkill(SkillCaster newSkill) {
        SkillCaster newSkillCaster = Instantiate(newSkill, this.transform);
        newSkillCaster.gameObject.SetActive(true);
        newSkillCaster.transform.position = this.FighterSkillCaster.transform.position;
        this.FighterSkillCaster = newSkillCaster;
    }

    public void SkillChange(bool isUpdate) {
        SkillCaster newSkill = isUpdate ? this.HeroUpdateSkillCaster : this.OriginFighterSkillCaster;
        SkillCaster oldSkill = isUpdate ? this.OriginFighterSkillCaster : this.HeroUpdateSkillCaster;
        
        newSkill.gameObject.SetActive(true);
        oldSkill.gameObject.SetActive(false);
        if(this.FighterSkillCaster) newSkill.transform.position = this.FighterSkillCaster.transform.position;
        this.FighterSkillCaster = newSkill;
        if(this.FighterSkillCaster) this.SkillNameText.SetSkillName(this.FighterSkillCaster.Data.Name);
    }

    public List<PassiveEntry> GetHeroPassiveEntries() {
        return this.EquipPassiveEntries;
    }

    protected override void Start(){
        base.Start();
        this.Move.Agent.enabled = false;
    }

    public void SetStartPosition() {
        this.StartPosition = this.transform.position;
    }

    public void CancelHeroDeploy() {
        this.HeroDeploy.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void Deploy(int deployAreaIndex) {
        this.SetStartPosition();
        this.DeployAreaIndex = deployAreaIndex;
        this.Move.Agent.enabled = true;
        this.HeroDeploy.gameObject.layer = LayerMask.NameToLayer("HeroDeploy");
        this.Renderer.ChangeColor(Color.white, false);
        
        HeroAudioData data = this.WarehouseData.GetHeroAudio(HeroAudioType.上阵);
        if (data != null) {
            AudioManager.Instance.SetDialog(data.Audio, false);
        }

        BattleManager.Instance.AddHero(this);
        if (HeroSelfPassiveEntries.Length != 0){
            foreach (PassiveEntry entry in HeroSelfPassiveEntries) {
                AddPassiveEntry(entry, true);
            }
        }
        BattleManager.Instance.LoadHeroPassiveEntry(this);
        BattleManager.Instance.ShowHeroDetail(this);
        BattleManager.Instance.OnBattleStart += UpdateByFighterTypeCountPropertyChange;
    }

    public void UndressSelfEntry(){
        for (int i = 0; i < SelfPassiveEntries.Count; ) {
            RemovePassiveEntry(SelfPassiveEntries[i], true);
        }
    }

    public void UpdateByFighterTypeCountPropertyChange() {
        BattleManager.Instance.OnBattleStart -= UpdateByFighterTypeCountPropertyChange;
        foreach (PassiveEntry passiveEntry in SelfPassiveEntries) {
            if (passiveEntry is not (HeroPropertyByFighterTypeCountPassiveEntry
                or SkillPropertyByFighterTypeCountPassiveEntry)) continue;
            passiveEntry.Destruct(this);
            passiveEntry.Construct(this);
        }
        
        foreach (PassiveEntry passiveEntry in EquipPassiveEntries) {
            if (passiveEntry is not (HeroPropertyByFighterTypeCountPassiveEntry
                or SkillPropertyByFighterTypeCountPassiveEntry)) continue;
            passiveEntry.Destruct(this);
            passiveEntry.Construct(this);
        }
    }

    public void AddPassiveEntry(PassiveEntry entry, bool isSelfOwned){
        if (!entry) return;
        if (isSelfOwned) {
            SelfPassiveEntries.Add(entry);
        } else {
            EquipPassiveEntries.Add(entry);
        }
        entry.Construct(this);
    }

    public void RemovePassiveEntry(PassiveEntry removeEntry, bool isSelfOwned){
        if (!removeEntry) return;
        removeEntry.Destruct(this);

        if (isSelfOwned) {
            SelfPassiveEntries.Remove(removeEntry);
        } else {
            EquipPassiveEntries.Remove(removeEntry);
        }
    }

    public string GetPassiveEntryDesc(){
        if (this.HeroSelfPassiveEntries.Length == 0) return "";
        return this.HeroSelfPassiveEntries[0].Data.Description;
    }
    
}
