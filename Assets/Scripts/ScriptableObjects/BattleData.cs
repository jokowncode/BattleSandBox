
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PhantomSpirit/BattleData", fileName = "BattleData")]
public class BattleData : ScriptableObject{
    public string BattleName;
    public List<EnemyDepartmentData> EnemiesInBattle;
}
