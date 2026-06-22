using UnityEngine;

public static class MathTools{
    public static Vector3 CalculateCirclePosition(Vector3 center, float angle, float radius, float navMeshDistance = 1.0f) {
        float rad = angle * Mathf.Deg2Rad;
        float x = center.x + radius * Mathf.Cos(rad);
        float z = center.z + radius * Mathf.Sin(rad);
        NavMeshTools.GetNavMeshPosition(new Vector3(x, center.y, z), navMeshDistance, out Vector3 finalPos);
        return finalPos;
    }
}