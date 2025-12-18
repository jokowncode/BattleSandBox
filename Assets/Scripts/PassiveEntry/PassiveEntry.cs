
using System;
using UnityEngine;

// TODO: Passive Entry Condition
public abstract class PassiveEntry : MonoBehaviour{

    [field: SerializeField] public PassiveEntryData Data{ get; private set; }
    [field: SerializeField] public PassiveEntry UpgradePassiveEntry { get; private set; }

    private int PassiveEntrySortCode = -1;
    
    public int GetSortCode() {
        if (PassiveEntrySortCode != -1) return this.PassiveEntrySortCode;
        if (!this.Data) return -1; 
        PassiveEntrySort[] sorts = this.Data.Sorts;
        if (sorts is not { Length: > 0 }) return -1;
        this.PassiveEntrySortCode = 0;
        foreach (PassiveEntrySort sort in sorts) {
            this.PassiveEntrySortCode |= (int)sort;
        }
        return this.PassiveEntrySortCode;
    }

    public abstract void Construct(Hero hero);
    public abstract void Destruct(Hero hero);

    public virtual bool Precondition(Hero hero){
        return true;
    }

}
