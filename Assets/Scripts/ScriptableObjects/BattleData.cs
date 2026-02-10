
using System.Collections.Generic;
using UnityEngine;

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
    public List<StoreGoodsData> FixedGetGoods;
    public bool RandomBloodBottle = true;
    public int MaxBloodBottleAmount = 2;
    public bool RandomEXP = true;
    public int MaxEXPAmount = 2;
    public bool RandomPassiveEntry = true;
    public int PassiveEntryAmount = 2;
    public int MaxPassiveEntryStar = 2;
}
