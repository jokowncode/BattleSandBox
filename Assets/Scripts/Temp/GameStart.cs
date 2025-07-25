
using System;
using UnityEngine;

public class GameStart : MonoBehaviour{

    [SerializeField] private Hero CHero;
    [SerializeField] private PassiveEntry[] Entries;

    private void Start(){
        Debug.Log($"Initial -> Cooldown: {CHero.Cooldown}");
        foreach (PassiveEntry entry in Entries){
            CHero.AddPassiveEntry(entry);
        }
        Debug.Log($"Current -> Cooldown: {CHero.Cooldown}");
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            Debug.Log($"Before Remove -> Cooldown: {CHero.Cooldown}");
            CHero.RemovePassiveEntry(0);    
            Debug.Log($"After Remove -> Cooldown: {CHero.Cooldown}");
        }

        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            Debug.Log($"Before Remove -> Cooldown: {CHero.Cooldown}");
            CHero.RemovePassiveEntry(1);
            Debug.Log($"After Remove -> Cooldown: {CHero.Cooldown}");
        }
    }
}

