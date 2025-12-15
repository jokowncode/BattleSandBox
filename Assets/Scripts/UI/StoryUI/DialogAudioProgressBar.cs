
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogAudioProgressBar : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI CurrentProgress;
    [SerializeField] private TextMeshProUGUI RemainingProgress;
    [SerializeField] private Image ProgressBar;
    
    private WaitForSeconds WaitTimer = new WaitForSeconds(1.0f);
    private float TotalSeconds = 0.0f;
    private float CurrentSeconds = 0.0f;
    
    private static string SecondsToTimeString(float seconds) {
        float m = Mathf.Floor(seconds / 60);
        float s = Mathf.Floor(seconds - m * 60);
        
        string mS = m >= 10.0f ? m.ToString() : "0" + m;
        string sS = s >= 10.0f ? s.ToString() : "0" + s;
        
        return $"{mS}:{sS}";
    }

    public void StartProgressBar(float totalSeconds) {
        this.CurrentProgress.text = "00:00";
        this.RemainingProgress.text = SecondsToTimeString(totalSeconds);
        this.ProgressBar.fillAmount = 0.0f;

        this.CurrentSeconds = 0.0f;
        this.TotalSeconds = totalSeconds;
        StartCoroutine(ProgressBarCoroutine());
    }

    private IEnumerator ProgressBarCoroutine() {
        while (this.CurrentSeconds <= this.TotalSeconds) {
            yield return this.WaitTimer;
            this.CurrentSeconds += 1.0f;
            
            this.CurrentProgress.text = SecondsToTimeString(this.CurrentSeconds);
            this.RemainingProgress.text = SecondsToTimeString(this.TotalSeconds - this.CurrentSeconds);
            this.ProgressBar.fillAmount = this.CurrentSeconds / this.TotalSeconds;
        }
        
        this.CurrentProgress.text = SecondsToTimeString(this.TotalSeconds);
        this.RemainingProgress.text = "00:00";
        this.ProgressBar.fillAmount = 1.0f;
    }

    public void StopProgressBar() {
        StopAllCoroutines();
    }

}


