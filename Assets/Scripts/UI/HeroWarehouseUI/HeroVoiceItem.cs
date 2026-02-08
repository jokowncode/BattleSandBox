
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroVoiceItem : MonoBehaviour {

    [SerializeField] private Button PlayAudioButton;
    [SerializeField] private TextMeshProUGUI AudioNameText;

    private HeroDisplayPanelUI ParentPanel;
    
    private void Start() {
        this.ParentPanel = this.GetComponentInParent<HeroDisplayPanelUI>();
    }

    public void SetAudio(HeroAudioData data) {
        this.AudioNameText.text = data.Audio.name;
        this.PlayAudioButton.onClick.AddListener(() => {
            if(this.ParentPanel) this.ParentPanel.ShowAudioDialog(data);
        });
    }
}


