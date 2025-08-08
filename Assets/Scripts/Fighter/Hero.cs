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

    protected override void Awake(){
        base.Awake();
        PassiveEntries = new List<PassiveEntry>();
        HeroRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start(){
        base.Start();
        this.Move.Agent.enabled = false;
    }

    public void Deploy(){
        this.Move.Agent.enabled = true;
        if(DeployHeroSfx)
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, DeployHeroSfx);

        if (HeroSelfPassiveEntries.Length != 0){
            foreach (PassiveEntry entry in HeroSelfPassiveEntries) {
                AddPassiveEntry(entry);
            }
        }
    }

    public void Undress(){
        for (int i = 0; i < PassiveEntries.Count; ) {
            RemovePassiveEntry(PassiveEntries[i]);
        }
    }

    public void AddPassiveEntry(PassiveEntry entry){
        PassiveEntries.Add(entry);
        entry.Construct(this);
    }

    public void RemovePassiveEntry(PassiveEntry removeEntry){
        removeEntry.Destruct(this);
        PassiveEntries.Remove(removeEntry);
    }

    public string GetPassiveEntryDesc(){
        string desc = "";
        int index = 1;
        foreach (PassiveEntry entry in HeroSelfPassiveEntries){
            desc += $"·{entry.Data.Description};\n";
            index++;
        }
        return desc;
    }
}
