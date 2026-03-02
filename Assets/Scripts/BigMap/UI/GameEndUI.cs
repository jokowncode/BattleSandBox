
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameEndUI : MonoBehaviour, IPointerClickHandler {

    public static GameEndUI Instance;
    
    [SerializeField] private CanvasGroup First;
    [SerializeField] private CanvasGroup Second;
    [SerializeField] private float FadeDuration = 0.5f;

    [SerializeField] private TextMeshProUGUI CurrentPlayTime;
    [SerializeField] private TypeWriter SecondContentTypeWriter;
    [TextArea] [SerializeField] private string SecondContent;

    private CanvasGroup UICanvasGroup;
    
    private void Awake() {
        if (Instance != null) {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        this.UICanvasGroup = this.GetComponent<CanvasGroup>();
    }

    public void Show() {
        long time = SaveDataManager.Instance.GetCurrentPlayTime();
        this.CurrentPlayTime.text = GetPlayTimeString(time);
        SaveDataManager.Instance.PlayerInBigMap.TransMove(false);
        StopAllCoroutines();
        StartCoroutine(this.CanvasGroupFadeCoroutine(this.UICanvasGroup, 0.0f, 1.0f));
    }
    
    private static string GetPlayTimeString(long seconds) {
        long hour = seconds / 3600;
        long minute = seconds / 60 - hour * 60;
        long second = seconds % 60;
        return $"{hour:00}h{minute:00}min{second:00}s";
    }

    private IEnumerator CanvasGroupFadeCoroutine(CanvasGroup canvasGroup, float start, float end) {
        for (float t = 0.0f; t <= this.FadeDuration; t += Time.deltaTime) {
            canvasGroup.alpha = Mathf.Lerp(start, end, t / this.FadeDuration);
            yield return null;
        }
        canvasGroup.alpha = end;
        canvasGroup.interactable = canvasGroup.alpha > 0.9f;
        canvasGroup.blocksRaycasts = canvasGroup.alpha > 0.9f;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (this.Second.alpha < 0.1f) {
            StartCoroutine(ShowSecondPanel());
        } else if(!this.SecondContentTypeWriter.IsDelayEnd) {
            this.SecondContentTypeWriter.EndText();
        } else {
            SaveDataManager.Instance.PlayerInBigMap.TransMove(true);
            StartCoroutine(CanvasGroupFadeCoroutine(this.UICanvasGroup, 1.0f, 0.0f));
        }
    }

    private IEnumerator ShowSecondPanel() {
        yield return CanvasGroupFadeCoroutine(this.First, 1.0f, 0.0f);
        yield return CanvasGroupFadeCoroutine(this.Second, 0.0f, 1.0f);
        this.SecondContentTypeWriter.Play(this.SecondContent, 1.0f, true, false);
    }
}


