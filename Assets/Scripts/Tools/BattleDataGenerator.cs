
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BattleDataGenerator : MonoBehaviour{

    [SerializeField] private Transform EnemyParent;
    [SerializeField] private string AssetName;

    [SerializeField] private BattleData RecoverBattleData;

    public void Generate(){
        if (!EnemyParent || EnemyParent.childCount == 0) return;
        BattleData data = ScriptableObject.CreateInstance<BattleData>();
        List<EnemyDepartmentData> enemyDepartmentDatas = new List<EnemyDepartmentData>();
        foreach (Transform enemyTrans in EnemyParent){
            if (enemyTrans.TryGetComponent(out Enemy enemy)) {
                Enemy prefab = PrefabUtility.GetCorrespondingObjectFromSource(enemy);
                enemyDepartmentDatas.Add(new EnemyDepartmentData{
                    Position = enemyTrans.position,
                    EnemyPrefab = prefab
                });
            }
        }
        data.EnemiesInBattle = enemyDepartmentDatas;
        string path = $"Assets/ScriptableObjects/BattleData/{AssetName}.asset";
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Success Generate Battle Data");
    }

    public void Recover(){
        if (!RecoverBattleData) return;
        Clear();
        List<EnemyDepartmentData> departmentAreaData = RecoverBattleData.EnemiesInBattle;
        foreach (EnemyDepartmentData data in departmentAreaData){
            Enemy enemy = Instantiate(data.EnemyPrefab, this.EnemyParent);
            enemy.transform.position = data.Position;
        }
    }

    public void Clear(){
        foreach (Transform enemyTrans in EnemyParent){
            DestroyImmediate(enemyTrans.gameObject);
        }
    }
}


