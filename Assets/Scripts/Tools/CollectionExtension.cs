using System.Collections.Generic;
using UnityEngine;

public static class CollectionExtension{
    public static int ClearNullKey<TKey, TValue>(this Dictionary<TKey, TValue> dict) where TKey : UnityEngine.Object {
        if (dict == null || dict.Count == 0) return 0;
        List<TKey> deadKeys = null;
        foreach (var key in dict.Keys) {
            if (!key) {
                deadKeys ??= new List<TKey>();
                deadKeys.Add(key);
            }
        }
        if (deadKeys == null) return 0;
        foreach (var k in deadKeys) dict.Remove(k);
        return deadKeys.Count;
    }
}