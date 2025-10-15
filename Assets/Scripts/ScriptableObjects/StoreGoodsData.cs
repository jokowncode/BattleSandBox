
using UnityEngine;

public enum GoodsType {
    Hero,
    EXP
}


[CreateAssetMenu(menuName = "PhantomSpirit/StoreGoods", fileName = "StoreGoodsData")]
public class StoreGoodsData : ScriptableObject {
    public GameObject GoodsPrefab;
    public GoodsType Type;
    public float Value;
    public float Money;
    public Sprite GoodsSprite;
    public Color GoodsColor;
}

