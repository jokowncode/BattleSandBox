using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Hero : Fighter{

    [SerializeField] private AudioClip DeployHeroSfx;
    [SerializeField] private PassiveEntry[] HeroSelfPassiveEntries;

    private List<PassiveEntry> EquipPassiveEntries;
    private List<PassiveEntry> SelfPassiveEntries;
    
    public SpriteRenderer HeroRenderer{ get; private set; }
    public int HeroAvailablePassiveEntrySortCode { get; private set; }
    public int DeployAreaIndex { get; private set; }
    public Vector3 StartPosition { get; private set; }

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

        if (HeroSelfPassiveEntries.Length != 0){
            foreach (PassiveEntry entry in HeroSelfPassiveEntries) {
                PassiveEntry addEntry = Instantiate(entry);
                AddPassiveEntry(addEntry, true);
            }
        }
        BattleManager.Instance.ShowHeroDetail(this);
        BattleManager.Instance.LoadHeroPassiveEntry(this);
    }

    public void Undress(){
        for (int i = 0; i < EquipPassiveEntries.Count; ) {
            RemovePassiveEntry(EquipPassiveEntries[i], false);
        }
        for (int i = 0; i < SelfPassiveEntries.Count; ) {
            RemovePassiveEntry(SelfPassiveEntries[i], true);
        }
    }

    public void AddPassiveEntry(PassiveEntry entry, bool isSelfOwned){
        if (!entry) return;
        entry.transform.parent = this.transform;
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
        Destroy(removeEntry.gameObject);
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
