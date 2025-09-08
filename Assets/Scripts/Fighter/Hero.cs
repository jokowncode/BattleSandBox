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

    private List<PassiveEntry> EquipPassiveEntries;
    private List<PassiveEntry> SelfPassiveEntries;
    
    public SpriteRenderer HeroRenderer{ get; private set; }
    public int HeroAvailablePassiveEntrySortCode { get; private set; }
    public int DeployAreaIndex { get; private set; }
    public Vector3 StartPosition { get; private set; }

    public Action<Hero> OnShowHeroDetail;
    public Dictionary<string, Object> Records = new();

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

    public List<PassiveEntry> GetHeroPassiveEntries() {
        return this.EquipPassiveEntries;
    }

    protected override void Start(){
        base.Start();
        this.Move.Agent.enabled = false;
    }

    public void Deploy(int deployAreaIndex) {
        this.StartPosition = this.transform.position;
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
