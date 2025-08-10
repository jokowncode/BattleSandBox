using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using static UnityEditor.Progress;
using UnityEngine.UIElements;



public class SmoothNormalsTool : EditorWindow
{
    private struct NormalWeight
    {
        public Vector3 normal;
        public float weight;
    }
    // 物体网格
    private Mesh targetMesh;

    // 物体原本法线查看
    private bool showVertexNormals = false;
    private bool showFaceNormals = false;
    private Color vertexNormalColor = Color.green;
    private Color faceNormalColor = Color.blue;

    // 平滑法线
    private string[] generateSmoothNormalsOptions = { "生成平滑法线", "导入法线预设" };
    private int generateSmoothNormalsIndex = 0;
    // 平滑法线方法
    private string[] smoothNormalsMethods = { "平均法线", "加权平均法线" };
    private int smoothNormalsMethodsIndex = 0;
    private bool showSmoothNormals = false;
    private bool isGenerateSmoothNormals;
    //private bool 
    // 存贮法线
    private Vector3[] savedVertices = null;
    private Vector3[] savedSmoothNormals = null;
    private Vector3[] savedSmoothNormalsTBN = null;
    


    [MenuItem("Tools/SmoothNormalsTool")]
    public static void ShowWindow()
    {
        GetWindow<SmoothNormalsTool>("SmoothNormals");
    }

    void OnGUI()
    {

        //////////////////////////
        ////  选择修改物体   ////
        ////////////////////////
        GUILayout.Label("选择场景中的物体进行操作:", EditorStyles.boldLabel);

        GameObject selected = Selection.activeGameObject;

        // 显示当前选择的物体信息
        EditorGUILayout.LabelField("当前被选中的物体: ",selected ? selected.name : "未选中场景中任何物体");

        // 获取Mesh组件
        if (selected)
        {
            MeshFilter meshFilter = selected.GetComponent<MeshFilter>();
            SkinnedMeshRenderer skinnedMesh = selected.GetComponent<SkinnedMeshRenderer>();

            targetMesh = meshFilter ? meshFilter.sharedMesh :
                skinnedMesh ? skinnedMesh.sharedMesh : null;
        }

        ////////////////////////
        ////  法线可视化   ////
        //////////////////////
        EditorGUILayout.Space();
        GUILayout.Label("可视化法线:", EditorStyles.boldLabel);
        // 颜色选择
        vertexNormalColor = EditorGUILayout.ColorField("顶点法线颜色:", vertexNormalColor);
        faceNormalColor = EditorGUILayout.ColorField("面法线颜色:", faceNormalColor);
        // 顶点法线开关
        showVertexNormals = GUILayout.Toggle(showVertexNormals,
            "显示顶点法线");
        // 面法线开关
        showFaceNormals = GUILayout.Toggle(showFaceNormals,
            "显示面法线");

        EditorGUILayout.Space();

        //////////////////////////
        ////  平滑法线操作   ////
        ////////////////////////

        GUILayout.Label("平滑法线:", EditorStyles.boldLabel);
        // 基础下拉列表
        generateSmoothNormalsIndex = EditorGUILayout.Popup(
            "生成法线方法：",
            generateSmoothNormalsIndex,
            generateSmoothNormalsOptions
        );

        if(generateSmoothNormalsIndex == 0)
        {
            GUILayout.Label("选择生成方法:", EditorStyles.boldLabel);
            smoothNormalsMethodsIndex = EditorGUILayout.Popup("平滑法线方法：", smoothNormalsMethodsIndex, smoothNormalsMethods);
            showSmoothNormals = GUILayout.Toggle(showSmoothNormals,"显示平滑后法线");
            isGenerateSmoothNormals = GUILayout.Toggle(isGenerateSmoothNormals, "允许输入");
            //saveNormalsSpaceIndex = EditorGUILayout.Popup("存储法线方法：", saveNormalsSpaceIndex, saveNormalsSpaceOption);
            // 禁用状态控制
            using (new EditorGUI.DisabledGroupScope(!isGenerateSmoothNormals&& Selection.activeGameObject!=null))
            {
                if (GUILayout.Button("生成平滑法线"))
                {
                    GenerateSmoothNormals();
                }
                if (GUILayout.Button("将法线传入网格UV7"))
                {
                    SetSmoothNromalsToUV7();
                }
            }

        }



        // 显示当前选择
        //EditorGUILayout.LabelField($"Selected: {generateSmoothNormalsOptions[generateSmoothNormalsIndex]}");


        EditorGUILayout.Space();


        // 场景视图刷新
        SceneView.RepaintAll();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (!targetMesh|| Selection.activeTransform==null) return;

        // 绘制顶点法线
        if (showVertexNormals)
        {
            Vector3[] vertices = targetMesh.vertices;
            Vector3[] normals = targetMesh.normals;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 worldPos = Selection.activeTransform.TransformPoint(vertices[i]);
                Vector3 worldNormal = Selection.activeTransform.TransformDirection(normals[i]);

                Handles.color = vertexNormalColor;
                Handles.DrawLine(worldPos, worldPos + worldNormal * 0.1f);
            }
        }

        // 绘制面法线
        if (showFaceNormals)
        {
            Vector3[] vertices = targetMesh.vertices;
            int[] triangles = targetMesh.triangles;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 a = Selection.activeTransform.TransformPoint(vertices[triangles[i]]);
                Vector3 b = Selection.activeTransform.TransformPoint(vertices[triangles[i + 1]]);
                Vector3 c = Selection.activeTransform.TransformPoint(vertices[triangles[i + 2]]);

                Vector3 center = (a + b + c) / 3;
                Vector3 normal = Vector3.Cross(b - a, c - a).normalized;

                Handles.color = faceNormalColor;
                Handles.DrawLine(center, center + normal * 0.2f);
            }
        }

        // 绘制面法线
        if (showSmoothNormals)
        {
            if (savedVertices == null) return;
            for (int i = 0; i < savedVertices.Length; i++)
            {
                Vector3 worldPos = Selection.activeTransform.TransformPoint(savedVertices[i]);
                Vector3 worldNormal = Selection.activeTransform.TransformDirection(savedSmoothNormals[i]);

                Handles.color = vertexNormalColor;
                Handles.DrawLine(worldPos, worldPos + worldNormal * 0.1f);
            }
        }

    }

    void GenerateSmoothNormals()
    {
        Dictionary<Vector3, List<NormalWeight>> normalDict = new Dictionary<Vector3, List<NormalWeight>>();
        var triangles = targetMesh.triangles;
        var vertices = targetMesh.vertices;
        var normals = targetMesh.normals;
        var tangents = targetMesh.tangents;

        savedVertices = vertices;
        savedSmoothNormals = new Vector3[vertices.Length];
        savedSmoothNormalsTBN = new Vector3[vertices.Length];

        // 遍历所有的三角形 给出每个顶点的作用法线及其权值
        for (int i = 0; i <= triangles.Length - 3; i += 3)
        {
            int[] triangle = new int[] { triangles[i], triangles[i + 1], triangles[i + 2] };
            for (int j = 0; j < 3; j++)
            {
                int vertexIndex = triangle[j];
                var vertex = vertices[vertexIndex];
                if (!normalDict.ContainsKey(vertex))
                {
                    normalDict.Add(vertex, new List<NormalWeight>());
                }

                NormalWeight nw;
                Vector3 lineA = Vector3.zero;
                Vector3 lineB = Vector3.zero;

                if (j == 0)
                {
                    lineA = vertices[triangle[1]] - vertex;
                    lineB = vertices[triangle[2]] - vertex;
                }
                else if (j == 1)
                {
                    lineA = vertices[triangle[2]] - vertex;
                    lineB = vertices[triangle[0]] - vertex;
                }
                else
                {
                    lineA = vertices[triangle[0]] - vertex;
                    lineB = vertices[triangle[1]] - vertex;
                }

                nw.normal = Vector3.Cross(lineA.normalized, lineB.normalized).normalized;
                nw.weight = 1;
                if(smoothNormalsMethodsIndex == 1)
                    nw.weight = Mathf.Acos(Mathf.Clamp(Vector3.Dot(lineA.normalized, lineB.normalized),-1,1));
                normalDict[vertex].Add(nw);

                //Vector3 debugVec = new Vector3(0.5f, 0.5f, 0.5f);
                //if(vertex == debugVec)
                //{
                //    Debug.Log("DebugVec:" + debugVec + " normal" +nw.normal);
                //}
            }
        }
        for (int i = 0; i < vertices.Length; i++)
        {

            Vector3 vertex = vertices[i];
            if (!normalDict.ContainsKey(vertex))
            {
                continue;
            }
            List<NormalWeight> normalList = normalDict[vertex];

            Vector3 smoothNormal = Vector3.zero;
            // 计算权值
            float weightSum = 0;
            for (int j = 0; j < normalList.Count; j++)
            {
                NormalWeight nw = normalList[j];
                weightSum += nw.weight;
            }
            for (int j = 0; j < normalList.Count; j++)
            {
                NormalWeight nw = normalList[j];
                smoothNormal += nw.normal * nw.weight / weightSum;
            }

            // 计算平滑法线
            smoothNormal = smoothNormal.normalized;
            savedSmoothNormals[i] = smoothNormal;

            // tbn存入法线坐标
            var normal = normals[i];
            var tangent = tangents[i];
            var binormal = (Vector3.Cross(normal, tangent) * tangent.w).normalized;
            var tbn = new Matrix4x4(tangent, binormal, normal, Vector3.zero);
            tbn = tbn.transpose;
            savedSmoothNormalsTBN[i] = tbn.MultiplyVector(savedSmoothNormals[i]).normalized;
            Debug.Log("Vertice Pos:" + vertex + "SmoothNormals"+ savedSmoothNormalsTBN[i]);

        }
    }

    void SetSmoothNromalsToUV7()
    {
        targetMesh.SetUVs(3, savedSmoothNormalsTBN);
    }

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}