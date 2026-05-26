using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    [SerializeField] private AudioClip EnterSound;
    [SerializeField] private AudioClip ClickSound;
    [SerializeField] private AudioClip ExitSound;
    
    [SerializeField] private Color NormalColor = Color.white;
    [SerializeField] private Color HoverColor = Color.yellow;

    private TextMeshProUGUI ButtonText;
    
    private void Awake(){
        this.ButtonText = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start(){
        if (ButtonText) ButtonText.color = NormalColor;
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        if (EnterSound){
            AudioManager.Instance.PlaySfx(EnterSound);
        }
        if (ButtonText) ButtonText.color = HoverColor;
    }
    
    public void OnPointerExit(PointerEventData eventData){
        if (ExitSound){
            AudioManager.Instance.PlaySfx(ExitSound);
        }
        if (ButtonText) ButtonText.color = NormalColor;
    }

    public void OnPointerClick(PointerEventData eventData){
        if (ClickSound){
            AudioManager.Instance.PlaySfx(ClickSound);
        }
    }
}
