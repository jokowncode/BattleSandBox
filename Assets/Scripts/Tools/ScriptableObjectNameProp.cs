
using UnityEngine;

public class ScriptableObjectNameProp : PropertyAttribute {
    public System.Type SoType;     
    public string FieldName;       
    
    public ScriptableObjectNameProp(System.Type soType, string fieldName) {
        this.SoType = soType;
        this.FieldName = fieldName;
    }
}


