
using System;
using UnityEngine;

public class FighterAnimationEvent : MonoBehaviour{

    public Action OnAttack;
    
    public void Attack(){
        OnAttack?.Invoke();
    }


}

