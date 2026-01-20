
using UnityEngine;

public enum GoodsType {
    Hero,
    EXP,
    PassiveEntry,
    BloodBottle
}


[CreateAssetMenu(menuName = "PhantomSpirit/StoreGoods", fileName = "StoreGoodsData")]
public class StoreGoodsData : ScriptableObject {
    public string GoodsName;
    public GoodsType Type;
    public float Value;
    public float Money;
    public Sprite GoodsSprite;
    public Color GoodsColor;
}

