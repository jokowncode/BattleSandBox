using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    [SerializeField] private ButtonTheme Theme;
    [SerializeField] private Color NormalColor = Color.white;
    [SerializeField] private Color HoverColor = Color.white;

    private ButtonTheme _Theme => Theme ? Theme : ButtonThemeDefaultProvider.DefaultButtonTheme;

    private TextMeshProUGUI ButtonText;
    
    private void Awake(){
        this.ButtonText = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start(){
        if (ButtonText) ButtonText.color = NormalColor;
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        if (_Theme?.EnterSound){
            AudioManager.Instance.PlaySfx(_Theme?.EnterSound);
        }
        if (ButtonText) ButtonText.color = HoverColor;
    }
    
    public void OnPointerExit(PointerEventData eventData){
        if (_Theme?.ExitSound){
            AudioManager.Instance.PlaySfx(_Theme?.ExitSound);
        }
        if (ButtonText) ButtonText.color = NormalColor;
    }

    public void OnPointerClick(PointerEventData eventData){
        if (_Theme?.ClickSound){
            AudioManager.Instance.PlaySfx(_Theme?.ClickSound);
        }
    }
}
