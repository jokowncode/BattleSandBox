

using UnityEngine;

[CreateAssetMenu(menuName = "DeckBreakers/HeroWarehouseData", fileName = "HeroWarehouseData")]
public class HeroWarehouseData : ScriptableObject {
    public string HeroChineseName;
    public string HeroEnglishName;
    public Sprite AvatarSprite;
    public Sprite[] MiddleSpriteAnims;
    [TextArea] public string HeroStory;
    public AudioClip[] HeroAudios;
}

