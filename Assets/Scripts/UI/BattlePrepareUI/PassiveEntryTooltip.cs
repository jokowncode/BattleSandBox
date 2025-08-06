using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PassiveEntryTooltip : MonoBehaviour{

    [SerializeField] private AudioClip ShowTipSfx;
    [SerializeField] private TextMeshProUGUI tooltipText;

    private RectTransform tooltipRect;

    private void Awake(){
        tooltipRect = this.GetComponent<RectTransform>();
        HideTooltip();
    }

    public void ShowTooltip(string message, Vector3 position){
        if(ShowTipSfx)
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, ShowTipSfx);
        
        this.gameObject.SetActive(true);
        tooltipText.text = message;
        tooltipRect.position = position;
    }

    public void HideTooltip(){
        this.gameObject.SetActive(false);
    }
}