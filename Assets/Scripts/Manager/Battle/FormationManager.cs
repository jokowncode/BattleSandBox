
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FormationManager : MonoBehaviour{
    
    public static FormationManager Instance;

    [SerializeField] private int Angle = 60;
    
    private Dictionary<Fighter, int> TargetRecords;
    public int MaxCount => 360 / Angle;
    
    private void Awake(){
        if (Instance != null){
            return;
        }
        Instance = this;
        TargetRecords = new Dictionary<Fighter, int>();
    }

    public bool ValidTarget(Fighter target){
        if (!TargetRecords.TryGetValue(target, out int count)) return true;
        return count < MaxCount;
    }

    public Vector3 GetFormationPosition(Fighter target, Fighter oldTarget, float radius){
        if (oldTarget && TargetRecords.ContainsKey(oldTarget)){
            TargetRecords[oldTarget]--;
            if(TargetRecords[oldTarget] <= 0) TargetRecords.Remove(oldTarget);
        }

        if (!TargetRecords.TryAdd(target, 1)) TargetRecords[target]++;

        int currentIndex = TargetRecords[target];
        float angle = ((currentIndex - 1) * Angle) % 360f;
        Vector3 theoreticalPos = CalculatePosition(target.transform.position, angle, radius);
        Vector3 validPosition = GetValidNavMeshPosition(theoreticalPos);
        return validPosition;
    }
    
    private Vector3 CalculatePosition(Vector3 center, float angle, float radius) {
        float rad = angle * Mathf.Deg2Rad;
        float x = center.x + radius * Mathf.Cos(rad);
        float z = center.z + radius * Mathf.Sin(rad);
        return new Vector3(x, center.y, z);
    }
    
    private Vector3 GetValidNavMeshPosition(Vector3 position){
        if (NavMesh.SamplePosition(position, out var hit, 1.0f, NavMesh.AllAreas)){
            return hit.position;
        }
        return position; 
    }
}

