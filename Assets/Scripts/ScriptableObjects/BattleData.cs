
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BattleData", fileName = "BattleData")]
public class BattleData : ScriptableObject {
    public List<EnemyDepartmentData> EnemiesInBattle;
}
