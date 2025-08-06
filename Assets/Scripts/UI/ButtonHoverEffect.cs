using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    
    [SerializeField] private Color NormalColor = Color.white;
    [SerializeField] private Color HoverColor = Color.yellow;

    private TextMeshProUGUI ButtonText;
    
    private void Awake(){
        this.ButtonText = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start(){
        ButtonText.color = NormalColor;
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        ButtonText.color = HoverColor;
    }
    
    public void OnPointerExit(PointerEventData eventData){
        ButtonText.color = NormalColor;
    }
}
