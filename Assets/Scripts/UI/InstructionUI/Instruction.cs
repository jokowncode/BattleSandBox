

using System;
using UnityEngine;
using UnityEngine.UI;

public enum NextInstructionMode {
    Click,
    Button
}

public class Instruction : MonoBehaviour{
    
    [SerializeField] private NextInstructionMode Mode = NextInstructionMode.Click;
    [SerializeField] private Button NextButton;
    
    private InstructionContainer Container;
    
    public void Show(InstructionContainer container) {
        this.Container = container;
        if (!this.Container) return;
        if (this.Container.Mask) {
            RectTransform targetRect = (RectTransform)this.transform;
            this.Container.Mask.Show(targetRect, targetRect.sizeDelta, this.Mode == NextInstructionMode.Click);
            if (Mode == NextInstructionMode.Click) 
                this.Container.Mask.OnInstructionMaskClicked += OnInstructionMaskClicked;   
        }

        if (Mode == NextInstructionMode.Button && this.NextButton) {
            this.NextButton.onClick.AddListener(this.Hide);
        }

        this.gameObject.SetActive(true);
    }

    private void OnInstructionMaskClicked() {
        this.Container.Mask.OnInstructionMaskClicked -= OnInstructionMaskClicked;
        this.Hide();
    }

    private void Hide() {
        if (Mode == NextInstructionMode.Button && this.NextButton) {
            this.NextButton.onClick.RemoveListener(this.Hide);
        }
        this.Container.Mask?.Hide();
        this.Container?.Next();
        this.gameObject.SetActive(false);
        this.Container = null;
    }
}


