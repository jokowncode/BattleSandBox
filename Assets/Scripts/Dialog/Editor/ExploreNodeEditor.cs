using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(ExploreNode))]
public class ExploreNodeEditor : NodeEditor {

    private int pickingIndex = -1;          // 当前正在为哪个 Mapping 拾取，-1 表示未拾取
    private const float MaxImageWidth = 260f;

    public override int GetWidth() => 300;  // 加宽节点，容纳图片

    private bool IsDragging = false;
    private Vector2 CurrentLeftTop = Vector2.zero;
    private Vector2 CurrentSize = Vector2.zero;

    public override void OnBodyGUI() {
        serializedObject.Update();

        // 1. 默认字段（ExploreCG、Mappings 列表、端口都会画出来）
        base.OnBodyGUI();

        ExploreNode node = (ExploreNode)target;
        Sprite sprite = node.ExploreCG;
        if (sprite == null) {
            EditorGUILayout.HelpBox("请先设置 ExploreCG", MessageType.Info);
            return;
        }

        // 2. 按原图长宽比计算绘制区域
        float aspect = sprite.rect.width / sprite.rect.height;
        float w = MaxImageWidth;
        float h = w / aspect;
        Rect imgRect = GUILayoutUtility.GetRect(w, h, GUILayout.ExpandWidth(false));

        // 3. 画 sprite 子区域
        DrawSprite(imgRect, sprite);

        // 4. 画已有 Mapping 标记点
        DrawMarkers(imgRect, node);

        // 5. 处理点击拾取
        HandlePick(imgRect, node);

        // 6. 操作区
        EditorGUILayout.Space();
        if (GUILayout.Button("+ 新增 Mapping 并拾取")) {
            node.Mappings.Add(new ExploreMapping());
            pickingIndex = node.Mappings.Count - 1;
            EditorUtility.SetDirty(node);
        }
        if (pickingIndex >= 0)
            EditorGUILayout.LabelField($"→ 点击图片设置 #{pickingIndex} 的坐标");

        // 每个 Mapping 一个"重新拾取"入口（可选）
        for (int i = 0; i < node.Mappings.Count; i++) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"#{i}", GUILayout.Width(28));
            EditorGUILayout.LabelField(node.Mappings[i].LeftTop.ToString(), GUILayout.Width(140));
            if (GUILayout.Button("拾取", GUILayout.Width(46))) pickingIndex = i;
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSprite(Rect rect, Sprite sprite) {
        Texture2D tex = sprite.texture;
        Rect tr = sprite.textureRect;
        Rect uv = new Rect(tr.x / tex.width, tr.y / tex.height,
                           tr.width / tex.width, tr.height / tex.height);
        GUI.DrawTextureWithTexCoords(rect, tex, uv, true);
        // 边框，方便看清点击区域
        Handles.BeginGUI();
        Handles.color = Color.gray;
        Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.gray);
        Handles.EndGUI();
    }

    private void DrawMarkers(Rect rect, ExploreNode node) {
        for (int i = 0; i < node.Mappings.Count; i++) {
            Vector2 loc = node.Mappings[i].LeftTop;
            Rect dot = new Rect(rect.x + loc.x * rect.width, rect.y + loc.y * rect.height, 
                node.Mappings[i].Size.x * rect.width, node.Mappings[i].Size.y * rect.height);
            Handles.DrawSolidRectangleWithOutline(dot, Color.clear, i == pickingIndex ? Color.yellow : Color.green);
        }
    }

    private void HandlePick(Rect rect, ExploreNode node) {
        Event e = Event.current;
        if (pickingIndex < 0 || pickingIndex >= node.Mappings.Count || e.type == EventType.Layout) {
            return;
        }
        
        if (e.type == EventType.MouseDown && e.button == 0 && rect.Contains(e.mousePosition) && !this.IsDragging) {
            this.IsDragging = true;
            float px = (e.mousePosition.x - rect.x) / rect.width;
            float py = (e.mousePosition.y - rect.y) / rect.height;
            this.CurrentLeftTop = new Vector2(px, py);   //相对左上角
        }

        if (e.type == EventType.MouseUp && e.button == 0 && this.IsDragging) {
            if (rect.Contains(e.mousePosition) && pickingIndex != -1) {
                node.Mappings[this.pickingIndex].LeftTop = this.CurrentLeftTop;
                node.Mappings[this.pickingIndex].Size = this.CurrentSize;
                EditorUtility.SetDirty(node);
            }
            this.Clear();
        }
        
        if (this.IsDragging) {
            if (!rect.Contains(e.mousePosition)) {
                this.Clear();
                return;
            }
            float px = (e.mousePosition.x - rect.x) / rect.width;
            float py = (e.mousePosition.y - rect.y) / rect.height;
            this.CurrentSize = new Vector2(Mathf.Abs(px - this.CurrentLeftTop.x), Mathf.Abs(py - this.CurrentLeftTop.y));

            Rect r = new Rect(rect.x + this.CurrentLeftTop.x * rect.width, rect.y + this.CurrentLeftTop.y * rect.height, 
                this.CurrentSize.x * rect.width, this.CurrentSize.y * rect.height);
            Handles.DrawSolidRectangleWithOutline(r, Color.clear, Color.blue);
        }

        if (e.type == EventType.MouseUp || e.type == EventType.MouseDown || e.type == EventType.MouseDrag) {
            e.Use();    
        }
    }

    private void Clear() {
        this.IsDragging = false;
        this.pickingIndex = -1;
        this.CurrentLeftTop = Vector2.zero;
        this.CurrentSize = Vector2.zero;
    }
}