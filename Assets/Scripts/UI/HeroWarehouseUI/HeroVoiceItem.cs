
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroVoiceItem : MonoBehaviour {

    [SerializeField] private Button PlayAudioButton;
    [SerializeField] private TextMeshProUGUI AudioNameText;

    public void SetAudio(HeroAudioData data, HeroDisplayPanelUI parentPanel) {
        this.AudioNameText.text = data.Audio.name;
        this.PlayAudioButton.onClick.AddListener(() => {
            if(parentPanel) parentPanel.ShowAudioDialog(data);
        });
    }
}


