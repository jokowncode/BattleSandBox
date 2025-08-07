
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SkillNameUI : MonoBehaviour{

    [SerializeField] private float FadeDuration = 0.5f;
    
    private TextMeshProUGUI SkillNameText;
    private CanvasGroup SkillNameCanvasGroup;

    private void Awake(){
        SkillNameText = GetComponentInChildren<TextMeshProUGUI>();
        SkillNameCanvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetSkillName(string skillName){
        SkillNameText.text = skillName;
    }

    public void Show(){ 
        // this.gameObject.SetActive(true);
        if (!this.gameObject.activeInHierarchy) return;
        StartCoroutine(FadeCoroutine(1.0f, this.FadeDuration));
    }

    private IEnumerator FadeCoroutine(float finalAlpha, float duration){
        float currentAlpha = this.SkillNameCanvasGroup.alpha;
        for (float t = 0.0f; t < duration; t += Time.deltaTime){
            this.SkillNameCanvasGroup.alpha = Mathf.Lerp(currentAlpha, finalAlpha, t / duration);
            yield return null;
        }
        this.SkillNameCanvasGroup.alpha = finalAlpha;
    }

    public void Hide(){
        // this.gameObject.SetActive(false);
        StartCoroutine(FadeCoroutine(0.0f, this.FadeDuration));
    }
}
