
using UnityEngine;

public enum GoodsType {
    None = -1,
    角色,
    经验,
    普通词条,
    血瓶,
    战术,
    特殊词条
}


[CreateAssetMenu(menuName = "DeckBreakers/StoreGoods", fileName = "StoreGoodsData")]
public class StoreGoodsData : ScriptableObject {
    public string GoodsName;
    public GoodsType Type;
    public float Value;
    public int Money;
}

