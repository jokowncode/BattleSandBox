using System;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Fighter{

    private List<PassiveEntry> PassiveEntries;

    private SkillCaster HeroSkill;

    protected override void Awake(){
        base.Awake();
        PassiveEntries = new List<PassiveEntry>();
        HeroSkill = GetComponent<SkillCaster>();
    }

    public void AddPassiveEntry(PassiveEntry entry){
        PassiveEntries.Add(entry);
        entry.Construct(this);
    }

    public void RemovePassiveEntry(int index){
        // TODO: Index Out of Range
        PassiveEntry entry = PassiveEntries[index];
        entry.Destruct(this);
        PassiveEntries.RemoveAt(index);
    }

    // TODO: TEMP
    public void CastSkill(){
        HeroSkill.CastSkill();   
    }
}
