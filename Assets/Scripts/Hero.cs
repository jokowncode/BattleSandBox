using System;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Fighter{
    
    private List<PassiveEntry> PassiveEntries;

    protected override void Awake(){
        base.Awake();
        PassiveEntries = new List<PassiveEntry>();
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
}
