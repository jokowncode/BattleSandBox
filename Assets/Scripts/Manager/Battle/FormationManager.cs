
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FormationManager : MonoBehaviour{
    
    public static FormationManager Instance;

    [SerializeField] private int MaxCount = 5;

    private int Angle => 180 / this.MaxCount;
    
    private void Awake(){
        if (Instance != null){
            return;
        }
        Instance = this;
    }
    
    public bool ValidTarget(Fighter target) {
        return target.AttackerCount < this.MaxCount;
    }

    public Vector3 GetFormationPosition(Fighter target, float radius) {
        float angle = (target.AttackerCount * Angle) % 360f;
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

