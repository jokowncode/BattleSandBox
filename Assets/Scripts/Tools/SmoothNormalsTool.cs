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
    // ��������
    private Mesh targetMesh;

    // ����ԭ�����߲鿴
    private bool showVertexNormals = false;
    private bool showFaceNormals = false;
    private Color vertexNormalColor = Color.green;
    private Color faceNormalColor = Color.blue;

    // ƽ������
    private string[] generateSmoothNormalsOptions = { "����ƽ������", "���뷨��Ԥ��" };
    private int generateSmoothNormalsIndex = 0;
    // ƽ�����߷���
    private string[] smoothNormalsMethods = { "ƽ������", "��Ȩƽ������" };
    private int smoothNormalsMethodsIndex = 0;
    private bool showSmoothNormals = false;
    private bool isGenerateSmoothNormals;
    //private bool 
    // ��������
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
        ////  ѡ���޸�����   ////
        ////////////////////////
        GUILayout.Label("ѡ�񳡾��е�������в���:", EditorStyles.boldLabel);

        GameObject selected = Selection.activeGameObject;

        // ��ʾ��ǰѡ���������Ϣ
        EditorGUILayout.LabelField("��ǰ��ѡ�е�����: ",selected ? selected.name : "δѡ�г������κ�����");

        // ��ȡMesh���
        if (selected)
        {
            MeshFilter meshFilter = selected.GetComponent<MeshFilter>();
            SkinnedMeshRenderer skinnedMesh = selected.GetComponent<SkinnedMeshRenderer>();

            targetMesh = meshFilter ? meshFilter.sharedMesh :
                skinnedMesh ? skinnedMesh.sharedMesh : null;
        }

        ////////////////////////
        ////  ���߿��ӻ�   ////
        //////////////////////
        EditorGUILayout.Space();
        GUILayout.Label("���ӻ�����:", EditorStyles.boldLabel);
        // ��ɫѡ��
        vertexNormalColor = EditorGUILayout.ColorField("���㷨����ɫ:", vertexNormalColor);
        faceNormalColor = EditorGUILayout.ColorField("�淨����ɫ:", faceNormalColor);
        // ���㷨�߿���
        showVertexNormals = GUILayout.Toggle(showVertexNormals,
            "��ʾ���㷨��");
        // �淨�߿���
        showFaceNormals = GUILayout.Toggle(showFaceNormals,
            "��ʾ�淨��");

        EditorGUILayout.Space();

        //////////////////////////
        ////  ƽ�����߲���   ////
        ////////////////////////

        GUILayout.Label("ƽ������:", EditorStyles.boldLabel);
        // ���������б�
        generateSmoothNormalsIndex = EditorGUILayout.Popup(
            "���ɷ��߷�����",
            generateSmoothNormalsIndex,
            generateSmoothNormalsOptions
        );

        if(generateSmoothNormalsIndex == 0)
        {
            GUILayout.Label("ѡ�����ɷ���:", EditorStyles.boldLabel);
            smoothNormalsMethodsIndex = EditorGUILayout.Popup("ƽ�����߷�����", smoothNormalsMethodsIndex, smoothNormalsMethods);
            showSmoothNormals = GUILayout.Toggle(showSmoothNormals,"��ʾƽ������");
            isGenerateSmoothNormals = GUILayout.Toggle(isGenerateSmoothNormals, "��������");
            //saveNormalsSpaceIndex = EditorGUILayout.Popup("�洢���߷�����", saveNormalsSpaceIndex, saveNormalsSpaceOption);
            // ����״̬����
            using (new EditorGUI.DisabledGroupScope(!isGenerateSmoothNormals&& Selection.activeGameObject!=null))
            {
                if (GUILayout.Button("����ƽ������"))
                {
                    GenerateSmoothNormals();
                }
                if (GUILayout.Button("�����ߴ�������UV7"))
                {
                    SetSmoothNromalsToUV7();
                }
            }

        }



        // ��ʾ��ǰѡ��
        //EditorGUILayout.LabelField($"Selected: {generateSmoothNormalsOptions[generateSmoothNormalsIndex]}");


        EditorGUILayout.Space();


        // ������ͼˢ��
        SceneView.RepaintAll();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (!targetMesh|| Selection.activeTransform==null) return;

        // ���ƶ��㷨��
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

        // �����淨��
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

        // �����淨��
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

        // �������е������� ����ÿ����������÷��߼���Ȩֵ
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
            // ����Ȩֵ
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

            // ����ƽ������
            smoothNormal = smoothNormal.normalized;
            savedSmoothNormals[i] = smoothNormal;

            // tbn���뷨������
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