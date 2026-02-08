using System;
using UnityEngine;
using System.Collections.Generic;

public class FlashPoint : MonoBehaviour{

    public float Damage{ get; private set; }
    private List<GameObject> flashPointsInRange = new List<GameObject>();

    public void SetDamage(float damage){
        this.Damage = damage;
    }

    public void Start(){
        FlashManager.Instance.RegisterPoint(this);
    }

    public void OnDisable(){
        if (!Application.isPlaying)
            return;
        
        if (this.gameObject.scene.isLoaded) {
            if (FlashManager.HasInstance)
                FlashManager.Instance.UnregisterPoint(this);
        }
    }
    
}
