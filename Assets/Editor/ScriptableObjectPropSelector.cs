

using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ScriptableObjectNameProp))]
public class ScriptableObjectPropSelector : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        ScriptableObjectNameProp attr = attribute as ScriptableObjectNameProp;
        
        if (property.propertyType != SerializedPropertyType.String) {
            EditorGUI.LabelField(position, label.text, "错误: 只能用于string类型");
            return;
        }
        
        string[] guids = AssetDatabase.FindAssets($"t:{attr.SoType.Name}");
        string[] options = new string[guids.Length];
        
        int selectedIndex = 0;
        
        for (int i = 0; i < guids.Length; i++) {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            
            if (obj && obj.GetType() == attr.SoType) {
                // 使用反射获取字段值
                var field = obj.GetType().GetField(attr.FieldName);
                if (field != null) {
                    string value = field.GetValue(obj)?.ToString() ?? $"Object_{i}";
                    options[i] = value;
                    
                    if (value == property.stringValue) {
                        selectedIndex = i;
                    }
                }
            }
        }
        
        int newIndex = EditorGUI.Popup(position, label.text, selectedIndex, options);
        property.stringValue = options[newIndex];
        property.serializedObject.ApplyModifiedProperties();
    }        
}
#endif

