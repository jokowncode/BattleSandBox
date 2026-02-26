using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class SmoothNormalToUV7 : MonoBehaviour
{
    public bool generateOnStart = true;
    public float positionTolerance = 0.0001f;

    void Start()
    {
        if (generateOnStart)
            Generate();
    }

    [ContextMenu("Generate Smooth Normal To UV7")]
    public void Generate()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();

        if (mf == null && smr == null)
            return;

        Mesh originalMesh = mf ? mf.sharedMesh : smr.sharedMesh;
        if (originalMesh == null)
            return;

        Mesh meshInstance = Instantiate(originalMesh);

        Vector3[] vertices = meshInstance.vertices;
        Vector3[] normals = meshInstance.normals;

        Vector3[] smooth = CalculateSmooth(vertices, normals);

        List<Vector4> uv7 = new List<Vector4>(vertices.Length);

        for (int i = 0; i < smooth.Length; i++)
        {
            Vector3 n = smooth[i];
            uv7.Add(new Vector4(n.x, n.y, n.z, 0));
        }

        meshInstance.SetUVs(7, uv7);

        if (mf)
            mf.mesh = meshInstance; // ⭐ 用 mesh 而不是 sharedMesh
        else
            smr.sharedMesh = meshInstance; // Skinned 只能用 sharedMesh
    }

    Vector3[] CalculateSmooth(Vector3[] vertices, Vector3[] normals)
    {
        Dictionary<Vector3, List<int>> groups = new Dictionary<Vector3, List<int>>();
        Vector3[] result = new Vector3[vertices.Length];

        float scale = 1f / positionTolerance;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 key = new Vector3(
                Mathf.Round(vertices[i].x * scale) / scale,
                Mathf.Round(vertices[i].y * scale) / scale,
                Mathf.Round(vertices[i].z * scale) / scale
            );

            if (!groups.TryGetValue(key, out var list))
            {
                list = new List<int>();
                groups.Add(key, list);
            }

            list.Add(i);
        }

        foreach (var pair in groups)
        {
            Vector3 avg = Vector3.zero;

            foreach (int index in pair.Value)
                avg += normals[index];

            avg.Normalize();

            foreach (int index in pair.Value)
                result[index] = avg;
        }

        return result;
    }
}