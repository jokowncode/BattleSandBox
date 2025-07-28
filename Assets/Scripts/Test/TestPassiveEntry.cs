
using System;
using UnityEngine;

public class TestPassiveEntry : MonoBehaviour{

    [SerializeField] private Hero CHero;
    [SerializeField] private PassiveEntry[] Entries;

    private void Start(){
        foreach (PassiveEntry entry in Entries){
            CHero.AddPassiveEntry(entry);
        }

    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            CHero.RemovePassiveEntry(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            CHero.RemovePassiveEntry(1);
        }
    }
}

