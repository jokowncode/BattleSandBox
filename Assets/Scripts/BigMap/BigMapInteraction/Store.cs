
using UnityEngine;

public class Store : InteractionObject{
    protected override void Interaction(){
        BigMapUIManager.Instance.TransitionStore(true);
    }
}

