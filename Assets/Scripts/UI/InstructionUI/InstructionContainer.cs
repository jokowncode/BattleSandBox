
using System;
using System.Collections.Generic;
using UnityEngine;


public class InstructionContainer : MonoBehaviour {

    [field: SerializeField] public InstructionMask Mask { get; private set; }

    private List<Instruction> Instructions;
    private int CurrentIndex = 0;

    public Action OnEndInstruction;
    
    public void ActivateInstruction() {
        this.Instructions = new();
        foreach (Transform child in this.transform) {
            if (child.TryGetComponent(out Instruction instruction)) {
                this.Instructions.Add(instruction);
            }
        }

        this.CurrentIndex = 0;
        if (this.Instructions.Count > 0) {
            this.gameObject.SetActive(true);
            this.Instructions[this.CurrentIndex].Show(this);
            if (SaveDataManager.Instance && SaveDataManager.Instance.PlayerInBigMap) {
                SaveDataManager.Instance.PlayerInBigMap.TransMove(false);
            }
        }
    }

    public void Next() {
        this.CurrentIndex++;
        if (this.CurrentIndex >= this.Instructions.Count) {
            this.EndInstruction();
            return;
        }
        this.Instructions[this.CurrentIndex].Show(this);
    }

    private void EndInstruction() {
        if (SaveDataManager.Instance && SaveDataManager.Instance.PlayerInBigMap) {
            SaveDataManager.Instance.PlayerInBigMap.TransMove(true);
        }
        this.gameObject.SetActive(false);
        OnEndInstruction?.Invoke();
    }
}



