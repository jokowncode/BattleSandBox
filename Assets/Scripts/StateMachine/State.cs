
using UnityEngine;
using UnityEngine.AI;

public class State : MonoBehaviour{
    public virtual void Construct(){ }
    public virtual void Execute(){ }
    public virtual void Destruct(){ }

    public virtual void Transition(){ }
}

