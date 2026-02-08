
using System;
using UnityEngine;

public abstract class PassiveEntry : MonoBehaviour{

    [field: SerializeField] public PassiveEntryData Data{ get; private set; }
    [field: SerializeField] public PassiveEntry UpgradePassiveEntry { get; private set; }
    
    public int GetSortCode() {
        // TODO: MYSTERIOUS BUG
        if (!this.Data) return -1; 
        PassiveEntrySort[] sorts = this.Data.Sorts;
        if (sorts is not { Length: > 0 }) return -1;
        int code = 0;
        foreach (PassiveEntrySort sort in sorts) {
            code |= (int)sort;
        }
        return code;
    }

    public abstract void Construct(Hero hero);
    public abstract void Destruct(Hero hero);

    public virtual bool Precondition(Hero hero){
        return true;
    }

}
