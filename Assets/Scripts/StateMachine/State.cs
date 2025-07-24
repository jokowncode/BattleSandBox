
using UnityEngine;
using UnityEngine.AI;

public class State : MonoBehaviour{

    protected Fighter Controller;

    protected virtual void Awake(){
        Controller = GetComponent<Fighter>();
    }

    public virtual void Construct(){ }
    public virtual void Execute(){ }
    public virtual void Destruct(){ }

    public virtual void Transition(){ }
}

