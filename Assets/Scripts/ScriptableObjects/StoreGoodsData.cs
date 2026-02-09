
using UnityEngine;

public enum GoodsType {
    None = -1,
    Hero,
    EXP,
    NormalPassiveEntry,
    BloodBottle,
    Tactic,
    SpecialPassiveEntry
}


[CreateAssetMenu(menuName = "DeckBreakers/StoreGoods", fileName = "StoreGoodsData")]
public class StoreGoodsData : ScriptableObject {
    public string GoodsName;
    public GoodsType Type;
    public float Value;
    public float Money;
    // public Sprite GoodsSprite;
    // public Sprite GoodsBackgroundSprite;
    public Color GoodsColor;
}

