
using System;
using UnityEngine;

public class Room : MonoBehaviour{

    [SerializeField] private AudioClip EnterSfx;

    private void OnTriggerEnter(Collider other){
        if(EnterSfx)
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, EnterSfx);
    }
}


