using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionSkillCaster : SkillCaster 
{
    [Header("Prefab Settings")]
    [SerializeField] private GameObject prefabToCreate;  // 要创建的Prefab
    [SerializeField] private string targetChildName;     // 目标子物体的名称
    
    
    protected override void Cast(Transform _){
        CreatePrefabOnTargetChild();
    }
    // 在目标子物体上创建Prefab
    public void CreatePrefabOnTargetChild()
    {
        if (prefabToCreate == null)
        {
            Debug.LogError("Prefab to create is not assigned!", this);
            return;
        }
        
        if (string.IsNullOrEmpty(targetChildName))
        {
            Debug.LogError("Target child name is not specified!", this);
            return;
        }
        
        // 查找目标子物体
        Transform targetChild = FindTargetChild();
        if (targetChild == null)
        {
            Debug.LogError($"Target child '{targetChildName}' not found!", this);
            return;
        }
        
        // 实例化Prefab
        GameObject instantiatedPrefab = Instantiate(prefabToCreate, targetChild);
        
        // 重置位置和旋转，使其与目标子物体对齐
        instantiatedPrefab.transform.localPosition = Vector3.zero;
        instantiatedPrefab.transform.localRotation = Quaternion.identity;
        instantiatedPrefab.transform.localScale = Vector3.one;
        
        Debug.Log($"Successfully created prefab on {targetChildName}", this);
    }
    
    // 通过名称查找目标子物体
    private Transform FindTargetChild()
    {
        return FindChildRecursive(transform, targetChildName);
    }
    
    // 递归查找子物体
    private Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;
            
            Transform result = FindChildRecursive(child, childName);
            if (result != null)
                return result;
        }
        return null;
    }
}
