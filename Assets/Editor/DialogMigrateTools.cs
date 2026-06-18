using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;

public static class DialogMigrateTools {

    [MenuItem("Tools/Dialog/Force Migrate All")]
    public static void ForceMigrateAll() {
        var guids = AssetDatabase.FindAssets("t:NodeGraph");
        int migratedNodeCount = 0;
        int touchedGraphCount = 0;

        try {
            for (int i = 0; i < guids.Length; i++) {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                EditorUtility.DisplayProgressBar(
                    "Migrate DialogNode",
                    $"[{i + 1}/{guids.Length}] {path}",
                    (float)i / guids.Length);

                var graph = AssetDatabase.LoadAssetAtPath<NodeGraph>(path);
                if (!graph) continue;

                bool dirty = false;
                foreach (var node in graph.nodes) {
                    if (node is DialogNode) {
                        EditorUtility.SetDirty(node);
                        dirty = true;
                        migratedNodeCount++;
                    }
                }

                if (dirty) {
                    EditorUtility.SetDirty(graph);
                    touchedGraphCount++;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        } finally {
            EditorUtility.ClearProgressBar();
        }

        Debug.Log($"✅ 已迁移 {migratedNodeCount} 个 DialogNode（涉及 {touchedGraphCount} 个 Graph）");
    }
}