
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VictoryFixedGoodsData {
    public StoreGoodsData Data;
    public int Count;
}

[CreateAssetMenu(menuName = "DeckBreakers/BattleData", fileName = "BattleData")]
public class BattleData : ScriptableObject {
    public SceneType BattleScene = SceneType.Battle_Normal;
    public int MaxHeroCount = 6;
    public string BattleName;
    public string BattleMessage;
    public Sprite BattleImage;
    public Sprite BattleBannarBackground;
    public string BattleText;
    public AudioClip BattleBGM;
    public float BondMultiplier = 1.0f;
    public List<EnemyDepartmentData> EnemiesInBattle;

    [Header("Victory Get")] 
    public int Money = 50;
    public List<VictoryFixedGoodsData> FixedGetGoods;
    public int RandomBloodBottleAmount = 3;
    public int RandomExpAmount = 3;
    public int RandomPassiveEntryMaxStar = 1;
    public int RandomPassiveEntryAmount = 3;
}
