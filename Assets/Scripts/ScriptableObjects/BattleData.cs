
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PhantomSpirit/BattleData", fileName = "BattleData")]
public class BattleData : ScriptableObject{
    public int MaxHeroCount = 6;
    public string BattleName;
    public string BattleMessage;
    public Sprite BattleImage;
    public Sprite BattleBannarBackground;
    public string BattleText;
    public AudioClip BattleBGM;
    public List<EnemyDepartmentData> EnemiesInBattle;
}
