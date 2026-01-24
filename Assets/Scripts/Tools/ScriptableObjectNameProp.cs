
using UnityEngine;

public class ScriptableObjectNameProp : PropertyAttribute {
    public System.Type SoType;     
    public string FieldName;
    public bool HasNull = false;
    
    public ScriptableObjectNameProp(System.Type soType, string fieldName, bool hasNull = false) {
        this.SoType = soType;
        this.FieldName = fieldName;
        this.HasNull = hasNull;
    }
}


