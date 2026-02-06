
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroVoiceItem : MonoBehaviour {

    [SerializeField] private Button PlayAudioButton;
    [SerializeField] private TextMeshProUGUI AudioNameText;

    public void SetAudio(AudioClip clip) {
        this.AudioNameText.text = clip.name;
        this.PlayAudioButton.onClick.AddListener(() => {
            AudioManager.Instance.SetDialog(clip, 1.0f);
        });
    }
}


