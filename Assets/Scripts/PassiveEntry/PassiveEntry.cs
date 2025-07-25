
using UnityEngine;

public abstract class PassiveEntry : MonoBehaviour{

    [SerializeField] private PassiveEntryData Data;
    
    public abstract void Construct(Hero hero);
    public abstract void Destruct(Hero hero);

    public virtual bool Precondition(Hero hero){
        return true;
    }

}
