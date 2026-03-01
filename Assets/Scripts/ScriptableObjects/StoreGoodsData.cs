
using UnityEngine;

public enum GoodsType {
    None = 0,
    角色 = 1 << 0,
    经验 = 1 << 1,
    普通词条 = 1 << 2,
    血瓶 = 1 << 3,
    战术 = 1 << 4,
    特殊词条 = 1 << 5,
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

