
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuffMiniData))]
public class BuffMiniDataEditor : Editor {

    private SerializedObject Obj;
    private BuffMiniData Target;

    private GUIStyle TitleStyle;

    private void OnEnable() {
        Obj = new SerializedObject(target);
        TitleStyle = new GUIStyle {
            fontStyle = FontStyle.Bold, 
            normal = new GUIStyleState { textColor = Color.white }
        };
    }

    public override void OnInspectorGUI() {
        // base.OnInspectorGUI();
        Obj.Update();
        Target = (BuffMiniData)target;
        
        GUILayout.Label("Property Change", this.TitleStyle);
        Target.ModifyWay = (PropertyModifyWay)EditorGUILayout.EnumPopup("ModifyWay", Target.ModifyWay);
        
        if (Target.ModifyWay == PropertyModifyWay.Percentage) {
            EditorGUILayout.PropertyField(Obj.FindProperty("PropertyRef"));
            Target.Ref = (BuffRef)EditorGUILayout.EnumPopup("Ref", Target.Ref);
            if (Target.Ref == BuffRef.Caster) {
                EditorGUILayout.PropertyField(Obj.FindProperty("CasterProperty"));
            } else {
                EditorGUILayout.PropertyField(Obj.FindProperty("TargetRefProperty"));
            }
        }
        
        EditorGUILayout.PropertyField(Obj.FindProperty("TargetUpdateProperty"));
        EditorGUILayout.PropertyField(Obj.FindProperty("ChangedValue"));
        EditorGUILayout.PropertyField(Obj.FindProperty("IsChangeProperty"));
        
        GUILayout.Space(20);
        GUILayout.Label("Particle", this.TitleStyle);
        EditorGUILayout.PropertyField(Obj.FindProperty("IsDestroyImmediate"));
        EditorGUILayout.PropertyField(Obj.FindProperty("DestroyDelay"));
        EditorGUILayout.PropertyField(Obj.FindProperty("EffectParticlePrefab"));

        if (Obj.hasModifiedProperties) {
            Obj.ApplyModifiedProperties();
        }
    }
}
