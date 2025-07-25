
using System;
using System.Reflection;

public static class ReflectionTools{

    public static T GetObjectProperty<T>(string propertyName, object obj){
        PropertyInfo info = obj.GetType().GetProperty(propertyName);
        return (T)info?.GetValue(obj);
    }

    public static void SetObjectProperty(string propertyName, object obj, object value){
        PropertyInfo info = obj.GetType().GetProperty(propertyName);
        info?.SetValue(obj, value);
    }
}

