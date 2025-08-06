
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleDataGenerator))]
public class BattleDataGeneratorCustomEditor : Editor {
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        BattleDataGenerator generator = (BattleDataGenerator)target;
        if (GUILayout.Button("生成")){
            generator.Generate();
        }
        
        if (GUILayout.Button("恢复")){
            generator.Recover();
        }
        
        if (GUILayout.Button("清空")){
            generator.Clear();
        }
    }
}

