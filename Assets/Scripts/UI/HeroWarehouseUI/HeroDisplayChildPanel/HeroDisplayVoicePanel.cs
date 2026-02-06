
using UnityEngine;

public class HeroDisplayVoicePanel : HeroDisplayChildPanel {

    [SerializeField] private Transform AudioContainer;
    [SerializeField] private HeroVoiceItem VoiceItemPrefab;
    
    protected override void ShowData(Hero hero) {
        foreach (Transform child in AudioContainer) {
            Destroy(child.gameObject);
        }

        AudioClip[] audios = hero.WarehouseData.HeroAudios;
        foreach (AudioClip clip in audios) {
            if (!clip) continue;
            HeroVoiceItem voiceItem = Instantiate(this.VoiceItemPrefab, AudioContainer);
            voiceItem.SetAudio(clip);
        }
    }
}

