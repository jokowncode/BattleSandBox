

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeroAudioData {
    public AudioClip Audio = null;
    [TextArea] public string AudioContent = "暂无";
}

[CreateAssetMenu(menuName = "DeckBreakers/HeroWarehouseData", fileName = "HeroWarehouseData")]
public class HeroWarehouseData : ScriptableObject {
    public string HeroChineseName;
    public string HeroEnglishName;
    public Sprite AvatarSprite;
    public Sprite[] MiddleSpriteAnims;
    [TextArea] public string HeroStory;
    public HeroAudioData[] HeroAudios;

    private Dictionary<HeroAudioType, HeroAudioData> HeroAudioMaps = new ();
    
    public HeroAudioData GetHeroAudio(HeroAudioType type) {
        if (this.HeroAudios == null || this.HeroAudios.Length == 0) return null;
        if (this.HeroAudioMaps.ContainsKey(type)) {
            return this.HeroAudioMaps[type];
        }

        foreach (HeroAudioData data in HeroAudios) {
            if (Enum.TryParse(data.Audio.name, true, out HeroAudioType audioType) && audioType == type) {
                this.HeroAudioMaps.Add(type, data);
                return data;
            }
        }
        return null;
    }
}

