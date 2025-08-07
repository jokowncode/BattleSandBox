using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    [SerializeField] private AudioClip EnterSound;
    [SerializeField] private AudioClip ClickSound;
    
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
        if (EnterSound){
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, EnterSound);
        }
        ButtonText.color = HoverColor;
    }
    
    public void OnPointerExit(PointerEventData eventData){
        ButtonText.color = NormalColor;
    }

    public void OnPointerClick(PointerEventData eventData){
        if (ClickSound){
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, ClickSound);
        }
    }
}
