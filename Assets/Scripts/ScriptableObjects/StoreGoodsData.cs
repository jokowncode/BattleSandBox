
using UnityEngine;

public enum GoodsType {
    None = 0,
    角色 = 1 << 0,
    羁绊经验书 = 1 << 1,
    普通芯片 = 1 << 2,
    血瓶 = 1 << 3,
    战术集 = 1 << 4,
    特殊芯片 = 1 << 5,
    复活书 = 1 << 6
}


[CreateAssetMenu(menuName = "DeckBreakers/StoreGoods", fileName = "StoreGoodsData")]
public class StoreGoodsData : ScriptableObject {
    public string GoodsName;
    public string GoodsShowName;
    public GoodsType Type;
    public float Value;
    public int Money;
}

