using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class ResponsiveGrid : MonoBehaviour
{
    public int columns = 4;         // 期望列数
    public int rows = 2;            // 可选：用于纵向自适应
    public Vector2 spacing = new Vector2(10, 10); // 单元格间距
    public Vector2 padding = new Vector2(20, 20); // 左右内边距（水平），上/下可以扩展

    private GridLayoutGroup grid;

    void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
        UpdateCellSize();
    }

    void Update()
    {
        // 可选：窗口变化时也更新
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateCellSize();
        }
    }

    private int lastWidth = 0;
    private int lastHeight = 0;

    void UpdateCellSize()
    {
        RectTransform rt = GetComponent<RectTransform>();
        float totalWidth = rt.rect.width;
        float totalHeight = rt.rect.height;

        float cellWidth = (totalWidth - padding.x * 2 - spacing.x * (columns - 1)) / columns;
        float cellHeight = (totalHeight - padding.y * 2 - spacing.y * (rows - 1)) / rows;

        grid.cellSize = new Vector2(cellWidth, cellHeight);
        grid.spacing = spacing;
        grid.padding = new RectOffset((int)padding.x, (int)padding.x, (int)padding.y, (int)padding.y);

        lastWidth = Screen.width;
        lastHeight = Screen.height;
    }
}
