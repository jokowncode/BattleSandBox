
using System;
using UnityEngine;

public class HeroDisplayVoicePanel : HeroDisplayChildPanel {

    [SerializeField] private Transform AudioContainer;
    [SerializeField] private HeroVoiceItem VoiceItemPrefab;

    private HeroDisplayPanelUI ParentPanel;
    
    private void Awake() {
        this.ParentPanel = this.GetComponentInParent<HeroDisplayPanelUI>();
    }

    protected override void ShowData(Hero hero) {
        foreach (Transform child in AudioContainer) {
            Destroy(child.gameObject);
        }
        
        HeroAudioData[] audios = hero.WarehouseData.HeroAudios;
        foreach (HeroAudioData data in audios) {
            if (!data.Audio) continue;
            HeroVoiceItem voiceItem = Instantiate(this.VoiceItemPrefab, AudioContainer);
            voiceItem.SetAudio(data, this.ParentPanel);
        }
    }
}

