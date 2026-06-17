using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

// List<T>
[Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> target;
    public List<T> ToList() { return target; }
 
    public Serialization(List<T> target)
    {
        this.target = target;
    }
}
 
// Dictionary<TKey, TValue>
[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver, IEnumerable
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();
    
    [SerializeField]
    private List<TValue> values = new List<TValue>();
    
    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
    
    // 实现字典接口
    public TValue this[TKey key]
    {
        get => dictionary[key];
        set => dictionary[key] = value;
    }
    
    public bool TryAdd(TKey key, TValue value) => dictionary.TryAdd(key, value);
    public void Add(TKey key, TValue value) => dictionary.Add(key, value);
    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
    public bool Remove(TKey key) => dictionary.Remove(key);
    public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

    public TValue GetValueOrDefault(TKey key, TValue defaultValue) => dictionary.GetValueOrDefault(key, defaultValue);

    public void Clear()
    {
        dictionary.Clear();
        keys.Clear();
        values.Clear();
    }
    
    public int Count => dictionary.Count;
    public Dictionary<TKey, TValue>.KeyCollection Keys => dictionary.Keys;
    public Dictionary<TKey, TValue>.ValueCollection Values => dictionary.Values;
    
    // 序列化前：将字典转换为两个List
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        
        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
    
    // 反序列化后：将两个List转换回字典
    public void OnAfterDeserialize()
    {
        dictionary.Clear();
        
        for (int i = 0; i < keys.Count && i < values.Count; i++)
        {
            if (keys[i] != null) // 防止空键
            {
                dictionary[keys[i]] = values[i];
            }
        }
    }

    public IEnumerator GetEnumerator() {
        return dictionary.GetEnumerator();
    }
}