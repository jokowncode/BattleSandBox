using UnityEngine;

public static class NavMeshTools{
    public static void GetNavMeshPosition(Vector3 currentPos, float maxDistance, out Vector3 navMeshPos){
        if (UnityEngine.AI.NavMesh.SamplePosition(currentPos, out var hit, maxDistance, UnityEngine.AI.NavMesh.AllAreas)){
            navMeshPos = hit.position;
            return;
        }
        navMeshPos = currentPos;
    }
}