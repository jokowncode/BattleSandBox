
using System;
using UnityEngine;

public class Room : MonoBehaviour{

    [SerializeField] private AudioClip EnterRoomSfx;

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && this.EnterRoomSfx){
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, EnterRoomSfx);
        }
    }
}
