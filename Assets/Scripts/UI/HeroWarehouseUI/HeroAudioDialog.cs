
using System;
using TMPro;
using UnityEngine;

public class HeroAudioDialog : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI AudioContentText;

    public void Show(HeroAudioData data) {
        this.AudioContentText.text = data.AudioContent;
        AudioManager.Instance.SetDialog(data.Audio);
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





