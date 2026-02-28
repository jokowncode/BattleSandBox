
using System;
using TMPro;
using UnityEngine;

public class HeroAudioDialog : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI AudioContentText;
    [SerializeField] private TextMeshProUGUI OtherLanguageText;

    public void Show(HeroAudioData data) {
        if (data == null) return;
        this.AudioContentText.text = data.AudioContent;
        this.OtherLanguageText.text = data.OtherLanguageContent;
        AudioManager.Instance.SetDialog(data.Audio, true);
        this.gameObject.SetActive(true);
    }

    private void Update() {
        if (AudioManager.Instance.DialogIsFinished) {
            this.Hide();
        }
    }

    public void Hide() {
        if(AudioManager.Instance) AudioManager.Instance.StopDialog();
        this.gameObject.SetActive(false);
    }
}





