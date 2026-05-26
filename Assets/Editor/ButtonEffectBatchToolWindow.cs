#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 批量给项目中的 Button 挂载 ButtonEffect 组件。
/// 策略：
///   - Prefab 资产优先扫描；
///   - 场景中属于 Prefab 实例的 Button 自动跳过（在 Prefab 资产层处理）；
///   - 已挂 ButtonEffect 的 Button 不会被重复添加。
/// </summary>
public class ButtonEffectBatchToolWindow : EditorWindow {

    private enum ButtonSource { PrefabAsset, SceneOnly }

    private class ButtonEntry {
        public bool Selected;
        public ButtonSource Source;
        public string ButtonName;
        public string LocationDisplay;     // Prefab 文件名 / 场景名
        public string AssetPath;           // Prefab 路径 / 场景路径
        public string HierarchyPath;       // 在根下的层级路径
        public bool HasButtonEffect;

        // 仅 SceneOnly 持有：场景中的 Button 直接引用
        public Button SceneButtonRef;
    }

    private readonly List<ButtonEntry> _entries = new List<ButtonEntry>();
    private Vector2 _scroll;
    private bool _hideAlreadyHasEffect = true;
    private bool _ignoreAssetStore = true;
    private string _searchText = string.Empty;

    private int _stat_PrefabTotal;
    private int _stat_SceneOnlyTotal;
    private int _stat_PrefabInstanceSkipped;

    [MenuItem("Tools/UI/Button Effect Batch Tool")]
    public static void Open() {
        var win = GetWindow<ButtonEffectBatchToolWindow>("ButtonEffect 批量挂载");
        win.minSize = new Vector2(720, 480);
    }

    private void OnGUI() {
        DrawToolbar();
        DrawStats();
        DrawList();
        DrawFooter();
    }

    private void DrawToolbar() {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        if (GUILayout.Button("扫描全项目", EditorStyles.toolbarButton, GUILayout.Width(100))) {
            Scan();
        }
        GUILayout.Space(8);
        _hideAlreadyHasEffect = GUILayout.Toggle(_hideAlreadyHasEffect, "仅显示缺失 ButtonEffect", EditorStyles.toolbarButton, GUILayout.Width(180));
        _ignoreAssetStore = GUILayout.Toggle(_ignoreAssetStore, "忽略 AssetStore", EditorStyles.toolbarButton, GUILayout.Width(120));
        GUILayout.FlexibleSpace();
        GUILayout.Label("搜索:", GUILayout.Width(40));
        _searchText = EditorGUILayout.TextField(_searchText, EditorStyles.toolbarSearchField, GUILayout.Width(180));
        EditorGUILayout.EndHorizontal();
    }

    private void DrawStats() {
        if (_entries.Count == 0) {
            EditorGUILayout.HelpBox("点击\"扫描全项目\"开始。Prefab 资产优先扫描；场景中已是 Prefab 实例的 Button 会被跳过。", MessageType.Info);
            return;
        }
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label($"Prefab 资产 Button: {_stat_PrefabTotal}", GUILayout.Width(180));
        GUILayout.Label($"Scene-Only Button: {_stat_SceneOnlyTotal}", GUILayout.Width(180));
        GUILayout.Label($"已跳过 Prefab 实例: {_stat_PrefabInstanceSkipped}", GUILayout.Width(180));
        EditorGUILayout.EndHorizontal();
    }

    private void DrawList() {
        // 列头
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        bool allSelected = AllVisibleSelected(out int visibleCount);
        bool newAll = GUILayout.Toggle(allSelected, GUIContent.none, GUILayout.Width(20));
        if (newAll != allSelected) SetAllVisibleSelected(newAll);
        GUILayout.Label("Button 名称", EditorStyles.miniBoldLabel, GUILayout.Width(180));
        GUILayout.Label("Source", EditorStyles.miniBoldLabel, GUILayout.Width(90));
        GUILayout.Label("Has Effect", EditorStyles.miniBoldLabel, GUILayout.Width(80));
        GUILayout.Label("Location / Path", EditorStyles.miniBoldLabel);
        EditorGUILayout.EndHorizontal();

        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        foreach (ButtonEntry e in _entries) {
            if (!IsVisible(e)) continue;
            DrawRow(e);
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawRow(ButtonEntry e) {
        EditorGUILayout.BeginHorizontal();

        bool canSelect = !e.HasButtonEffect;
        using (new EditorGUI.DisabledScope(!canSelect)) {
            e.Selected = GUILayout.Toggle(e.Selected && canSelect, GUIContent.none, GUILayout.Width(20));
        }

        if (GUILayout.Button(e.ButtonName, EditorStyles.label, GUILayout.Width(180))) {
            PingAndSelect(e);
        }

        GUI.color = e.Source == ButtonSource.PrefabAsset ? new Color(0.7f, 0.85f, 1f) : new Color(1f, 0.85f, 0.7f);
        GUILayout.Label(e.Source.ToString(), GUILayout.Width(90));
        GUI.color = Color.white;

        GUI.color = e.HasButtonEffect ? new Color(0.6f, 1f, 0.6f) : new Color(1f, 0.7f, 0.7f);
        GUILayout.Label(e.HasButtonEffect ? "✔ 有" : "✘ 无", GUILayout.Width(80));
        GUI.color = Color.white;

        GUILayout.Label($"{e.LocationDisplay}  →  {e.HierarchyPath}", EditorStyles.miniLabel);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawFooter() {
        EditorGUILayout.BeginHorizontal();
        int selectedCount = _entries.Count(e => e.Selected && !e.HasButtonEffect);
        GUILayout.Label($"已勾选: {selectedCount}", GUILayout.Width(100));
        GUILayout.FlexibleSpace();
        using (new EditorGUI.DisabledScope(selectedCount == 0)) {
            if (GUILayout.Button($"给勾选的 {selectedCount} 个按钮添加 ButtonEffect", GUILayout.Height(28), GUILayout.Width(280))) {
                ApplyAddButtonEffect();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    // ================== 扫描 ==================

    private void Scan() {
        _entries.Clear();
        _stat_PrefabTotal = 0;
        _stat_SceneOnlyTotal = 0;
        _stat_PrefabInstanceSkipped = 0;

        try {
            ScanPrefabAssets();
            ScanOpenedScenes();
        } finally {
            EditorUtility.ClearProgressBar();
        }

        Debug.Log($"[ButtonEffectBatchTool] 扫描完成：Prefab={_stat_PrefabTotal}, SceneOnly={_stat_SceneOnlyTotal}, 跳过 Prefab 实例={_stat_PrefabInstanceSkipped}");
    }

    private void ScanPrefabAssets() {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        for (int i = 0; i < guids.Length; i++) {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            if (ShouldIgnorePath(path)) continue;

            EditorUtility.DisplayProgressBar("扫描 Prefab", path, (float)i / guids.Length);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (!prefab) continue;
            Button[] buttons = prefab.GetComponentsInChildren<Button>(true);
            foreach (Button btn in buttons) {
                _entries.Add(new ButtonEntry {
                    Selected = false,
                    Source = ButtonSource.PrefabAsset,
                    ButtonName = btn.name,
                    LocationDisplay = System.IO.Path.GetFileName(path),
                    AssetPath = path,
                    HierarchyPath = GetTransformPath(btn.transform, prefab.transform),
                    HasButtonEffect = btn.GetComponent<ButtonEffect>() != null,
                    SceneButtonRef = null,
                });
                _stat_PrefabTotal++;
            }
        }
    }

    private void ScanOpenedScenes() {
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            foreach (GameObject root in scene.GetRootGameObjects()) {
                Button[] buttons = root.GetComponentsInChildren<Button>(true);
                foreach (Button btn in buttons) {
                    if (PrefabUtility.IsPartOfPrefabInstance(btn.gameObject)) {
                        _stat_PrefabInstanceSkipped++;
                        continue;
                    }
                    _entries.Add(new ButtonEntry {
                        Selected = false,
                        Source = ButtonSource.SceneOnly,
                        ButtonName = btn.name,
                        LocationDisplay = scene.name + ".unity",
                        AssetPath = scene.path,
                        HierarchyPath = GetTransformPath(btn.transform, null),
                        HasButtonEffect = btn.GetComponent<ButtonEffect>() != null,
                        SceneButtonRef = btn,
                    });
                    _stat_SceneOnlyTotal++;
                }
            }
        }
    }

    // ================== 应用 ==================

    private void ApplyAddButtonEffect() {
        var targets = _entries.Where(e => e.Selected && !e.HasButtonEffect).ToList();
        if (targets.Count == 0) return;

        if (!EditorUtility.DisplayDialog(
                "确认操作",
                $"将给 {targets.Count} 个按钮添加 ButtonEffect 组件。\n\n" +
                $"  - Prefab 资产: {targets.Count(t => t.Source == ButtonSource.PrefabAsset)}\n" +
                $"  - Scene-Only:  {targets.Count(t => t.Source == ButtonSource.SceneOnly)}\n\n" +
                $"Prefab 资产的修改不可 Undo，请确保已提交版本管理。",
                "确认", "取消")) {
            return;
        }

        int success = 0;
        int failed = 0;
        try {
            // Prefab 按 path 分组，减少 LoadPrefabContents 次数
            var prefabGroups = targets
                .Where(t => t.Source == ButtonSource.PrefabAsset)
                .GroupBy(t => t.AssetPath);

            int progressIdx = 0;
            int progressTotal = targets.Count;

            foreach (var group in prefabGroups) {
                EditorUtility.DisplayProgressBar("应用中", $"Prefab: {group.Key}", (float)progressIdx / progressTotal);
                if (ApplyToPrefab(group.Key, group.ToList(), out int addedInThis)) {
                    success += addedInThis;
                } else {
                    failed += group.Count();
                }
                progressIdx += group.Count();
            }

            foreach (var entry in targets.Where(t => t.Source == ButtonSource.SceneOnly)) {
                EditorUtility.DisplayProgressBar("应用中", $"Scene: {entry.LocationDisplay}", (float)progressIdx / progressTotal);
                if (ApplyToSceneButton(entry)) success++;
                else failed++;
                progressIdx++;
            }

            AssetDatabase.SaveAssets();
        } finally {
            EditorUtility.ClearProgressBar();
        }

        Debug.Log($"[ButtonEffectBatchTool] 应用完成：成功 {success}，失败 {failed}");
        Scan();
    }

    private bool ApplyToPrefab(string prefabPath, List<ButtonEntry> entries, out int added) {
        added = 0;
        GameObject root = PrefabUtility.LoadPrefabContents(prefabPath);
        if (!root) return false;
        try {
            foreach (var entry in entries) {
                Transform t = ResolveByPath(root.transform, entry.HierarchyPath);
                if (!t) {
                    Debug.LogWarning($"[ButtonEffectBatchTool] 在 {prefabPath} 中找不到 {entry.HierarchyPath}");
                    continue;
                }
                Button btn = t.GetComponent<Button>();
                if (!btn) continue;
                if (btn.GetComponent<ButtonEffect>()) continue;

                btn.gameObject.AddComponent<ButtonEffect>();
                added++;
            }
            if (added > 0) PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
        } finally {
            PrefabUtility.UnloadPrefabContents(root);
        }
        return true;
    }

    private bool ApplyToSceneButton(ButtonEntry entry) {
        if (!entry.SceneButtonRef) return false;
        if (entry.SceneButtonRef.GetComponent<ButtonEffect>()) return true;

        Undo.AddComponent<ButtonEffect>(entry.SceneButtonRef.gameObject);
        EditorSceneManager.MarkSceneDirty(entry.SceneButtonRef.gameObject.scene);
        return true;
    }

    // ================== 工具方法 ==================

    private bool ShouldIgnorePath(string path) {
        if (string.IsNullOrEmpty(path)) return true;
        if (_ignoreAssetStore && path.StartsWith("Assets/AssetStore/", System.StringComparison.OrdinalIgnoreCase)) return true;
        if (path.StartsWith("Packages/")) return true;
        return false;
    }

    private bool IsVisible(ButtonEntry e) {
        if (_hideAlreadyHasEffect && e.HasButtonEffect) return false;
        if (!string.IsNullOrEmpty(_searchText)) {
            string s = _searchText.ToLowerInvariant();
            if (!e.ButtonName.ToLowerInvariant().Contains(s) &&
                !e.LocationDisplay.ToLowerInvariant().Contains(s)) return false;
        }
        return true;
    }

    private bool AllVisibleSelected(out int visibleCount) {
        visibleCount = 0;
        bool all = true;
        foreach (var e in _entries) {
            if (!IsVisible(e) || e.HasButtonEffect) continue;
            visibleCount++;
            if (!e.Selected) all = false;
        }
        return visibleCount > 0 && all;
    }

    private void SetAllVisibleSelected(bool value) {
        foreach (var e in _entries) {
            if (!IsVisible(e) || e.HasButtonEffect) continue;
            e.Selected = value;
        }
    }

    private void PingAndSelect(ButtonEntry e) {
        if (e.Source == ButtonSource.SceneOnly && e.SceneButtonRef) {
            Selection.activeObject = e.SceneButtonRef.gameObject;
            EditorGUIUtility.PingObject(e.SceneButtonRef.gameObject);
            return;
        }

        // PrefabAsset：选中 Prefab 资产，Inspector 会显示其内部结构
        var prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(e.AssetPath);
        if (prefabAsset) {
            Selection.activeObject = prefabAsset;
            EditorGUIUtility.PingObject(prefabAsset);
        }
    }

    private static string GetTransformPath(Transform target, Transform stopAt) {
        if (!target) return string.Empty;
        string path = target.name;
        Transform cur = target.parent;
        while (cur != null && cur != stopAt) {
            path = cur.name + "/" + path;
            cur = cur.parent;
        }
        return path;
    }

    private static Transform ResolveByPath(Transform root, string path) {
        if (string.IsNullOrEmpty(path)) return null;
        // path 不含 root 自身
        string[] parts = path.Split('/');
        Transform cur = root;
        int startIndex = parts[0] == root.name ? 1 : 0;
        for (int i = startIndex; i < parts.Length; i++) {
            Transform next = cur.Find(parts[i]);
            if (!next) {
                // 兜底：递归找同名节点
                next = FindRecursive(cur, parts[i]);
                if (!next) return null;
            }
            cur = next;
        }
        return cur;
    }

    private static Transform FindRecursive(Transform parent, string name) {
        if (parent.name == name) return parent;
        for (int i = 0; i < parent.childCount; i++) {
            var found = FindRecursive(parent.GetChild(i), name);
            if (found) return found;
        }
        return null;
    }
}
#endif
