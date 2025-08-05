using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hero : Fighter{

    [SerializeField] private AudioClip DeployHeroSfx;

    private List<PassiveEntry> PassiveEntries;
    private NavMeshAgent HeroAgent;

    public SpriteRenderer HeroRenderer{ get; private set; }

    protected override void Awake(){
        base.Awake();
        PassiveEntries = new List<PassiveEntry>();
        HeroAgent = GetComponent<NavMeshAgent>();
        HeroRenderer = GetComponentInChildren<SpriteRenderer>();
        this.HeroAgent.enabled = false;
    }

    public void Deploy(){
        this.HeroAgent.enabled = true;
        if(DeployHeroSfx)
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, DeployHeroSfx);
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
