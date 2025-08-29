using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Hero : Fighter{

    [SerializeField] private AudioClip DeployHeroSfx;
    [SerializeField] private PassiveEntry[] HeroSelfPassiveEntries;

    private List<PassiveEntry> PassiveEntries;

    public SpriteRenderer HeroRenderer{ get; private set; }
    public int HeroAvailablePassiveEntrySortCode { get; private set; }
    public int DeployAreaIndex { get; private set; }

    protected override void Awake(){
        base.Awake();
        PassiveEntries = new List<PassiveEntry>();
        HeroRenderer = GetComponentInChildren<SpriteRenderer>();

        if (this.FighterSkillCaster) {
            this.HeroAvailablePassiveEntrySortCode =
                (int)PassiveEntrySort.General | (int)PassiveEntrySort.Talent | (int)this.FighterSkillCaster.Sort;
        }
        
    }

    protected override void Start(){
        base.Start();
        this.Move.Agent.enabled = false;
    }

    public void Deploy(int deployAreaIndex) {
        this.DeployAreaIndex = deployAreaIndex;
        this.Move.Agent.enabled = true;
        if(DeployHeroSfx)
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, DeployHeroSfx);

        if (HeroSelfPassiveEntries.Length != 0){
            foreach (PassiveEntry entry in HeroSelfPassiveEntries) {
                AddPassiveEntry(entry);
            }
        }
        BattleManager.Instance.ShowHeroDetail(this);
    }

    public void Undress(){
        for (int i = 0; i < PassiveEntries.Count; ) {
            RemovePassiveEntry(PassiveEntries[i]);
        }
    }

    public void AddPassiveEntry(PassiveEntry entry){
        if (!entry) return;
        PassiveEntry passiveEntry = Instantiate(entry, this.transform);
        PassiveEntries.Add(passiveEntry);
        passiveEntry.Construct(this);
    }

    public void RemovePassiveEntry(PassiveEntry removeEntry){
        if (!removeEntry) return;
        removeEntry.Destruct(this);
        PassiveEntries.Remove(removeEntry);
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
