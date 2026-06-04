using System.Collections;
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

    public void ShowTooltip(string message, Vector3 position) {
        if(ShowTipSfx)
            AudioManager.Instance.PlayUI(ShowTipSfx);
        
        this.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(SetTextCoroutine(message, position));
    }

    private IEnumerator SetTextCoroutine(string message, Vector3 position) {
        tooltipText.text = message;
        tooltipRect.position = position;
        yield return null;
        Vector2 size = this.tooltipRect.sizeDelta;
        size.y = tooltipText.preferredHeight + 70.0f;
        this.tooltipRect.sizeDelta = size;
    }

    public void HideTooltip(){
        this.gameObject.SetActive(false);
    }
}