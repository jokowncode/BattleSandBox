using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

public class Hero : Fighter{

    [SerializeField] private AudioClip DeployHeroSfx;
    [SerializeField] private PassiveEntry[] HeroSelfPassiveEntries;
    [field: SerializeField] public SkillCaster HeroUpdateSkillCaster { get; private set; }
    [SerializeField] private GameObject Shadow;
    
    [Header("Trail")]
    [SerializeField] private SimpleProjectile SkillTrailPrefab;
    
    private List<PassiveEntry> EquipPassiveEntries;
    private List<PassiveEntry> SelfPassiveEntries;
    
    public SpriteRenderer HeroRenderer{ get; private set; }
    public int HeroAvailablePassiveEntrySortCode { get; private set; }
    public int DeployAreaIndex { get; private set; }
    public Vector3 StartPosition { get; private set; }

    public Action<Hero> OnShowHeroDetail;
    public Dictionary<string, Object> Records = new();

    public int MergeGroupIndex { get; set; } = -1;
    public bool IsOriginExist = false;

    protected override void Awake(){
        base.Awake();
        EquipPassiveEntries = new List<PassiveEntry>();
        SelfPassiveEntries = new List<PassiveEntry>();
        HeroRenderer = GetComponentInChildren<SpriteRenderer>();

        if (this.FighterSkillCaster) {
            this.HeroAvailablePassiveEntrySortCode =
                (int)PassiveEntrySort.General | (int)PassiveEntrySort.Talent | (int)this.FighterSkillCaster.Sort;
        }
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
    
    public void ChangePositionWithTrail(Vector3 targetPosition) {
        if (SkillTrailPrefab) {
            SimpleProjectile projectile = Instantiate(SkillTrailPrefab, this.Center.position, Quaternion.identity);
            projectile.SetFlightParameters(this.Center.position, targetPosition, this);
        } else {
            this.transform.position = targetPosition; 
        }
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

    public void TransitionShow(bool show) {
        this.IsDisappear = !show;
        string layerName = show ? "Hero" : "HideLayer";
        string uiLayerName = show ? "UI" : "HideLayer";
        
        this.gameObject.layer = LayerMask.NameToLayer(layerName);
        this.HeroRenderer.gameObject.layer = LayerMask.NameToLayer(layerName);
        this.Shadow.layer = LayerMask.NameToLayer(layerName);
        this.FighterCanvas.gameObject.layer = LayerMask.NameToLayer(uiLayerName);

        foreach (Transform child in this.Center) {
            child.gameObject.SetActive(show);
        }
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

    public void Deploy(int deployAreaIndex) {
        this.SetStartPosition();
        this.DeployAreaIndex = deployAreaIndex;
        this.Move.Agent.enabled = true;
        if(DeployHeroSfx)
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, DeployHeroSfx);

        BattleManager.Instance.AddHero(this);
        if (HeroSelfPassiveEntries.Length != 0){
            foreach (PassiveEntry entry in HeroSelfPassiveEntries) {
                AddPassiveEntry(entry, true);
            }
        }
        BattleManager.Instance.ShowHeroDetail(this);
        BattleManager.Instance.LoadHeroPassiveEntry(this);
    }

    public void UndressSelfEntry(){
        for (int i = 0; i < SelfPassiveEntries.Count; ) {
            RemovePassiveEntry(SelfPassiveEntries[i], true);
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
        string desc = "";
        foreach (PassiveEntry entry in HeroSelfPassiveEntries){
            if (!entry) continue;
            desc += $"·{entry.Data.Description};\n";
        }
        return desc;
    }
    
}
