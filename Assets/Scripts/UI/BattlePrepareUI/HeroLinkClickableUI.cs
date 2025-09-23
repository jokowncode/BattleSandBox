using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroLinkClickableUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    
    public Hero Hero;
    private void Awake(){
        //PassiveEntryRect = this.GetComponent<RectTransform>();
    }

    private void Start(){

    }

    public void OnPointerEnter(PointerEventData eventData){

    }

    public void OnPointerExit(PointerEventData eventData){
 
    }

    public void OnPointerClick(PointerEventData eventData){
            
            Debug.Log("Clicked: "+Hero.name);
            if (HeroLinkUI.Instance.AssignHeroToSelectedSlot(Hero))
            {
                Destroy(this.gameObject);
            }
            //HeroLinkUI.Instance
    }
}
