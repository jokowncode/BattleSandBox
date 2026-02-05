using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class UISelectableShaker : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("Selection")]
    public float hoverOffsetY = 40f;
    public float moveSpeed = 12f;
    public float value = 1f;

    [Header("Shake")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 10f;

    RectTransform rect;

    // selection
    float currentSelectOffset;
    float targetSelectOffset;
    bool isSelected;

    // shake
    Vector2 shakeOffset;
    bool isShaking;

    // 死亡状态
    private bool isAlive = true;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        Vector2 layoutBase = rect.anchoredPosition - Vector2.up * currentSelectOffset - shakeOffset;
        
        currentSelectOffset = Mathf.Lerp(currentSelectOffset, targetSelectOffset, Time.deltaTime * moveSpeed);
        
        rect.anchoredPosition = layoutBase + Vector2.up * currentSelectOffset + shakeOffset;
    }

    // ===== Selection =====
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isAlive || value != 1 || isSelected) return;
        targetSelectOffset = hoverOffsetY;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected) return;
        targetSelectOffset = 0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isAlive || value != 1) return;

        if (!isSelected)
        {
            // 尝试选中
            if (UISelectionManager.Instance.TrySelect(this))
            {
                isSelected = true;
                targetSelectOffset = hoverOffsetY;
            }
        }
        else
        {
            // 取消选中
            UISelectionManager.Instance.Unselect(this);
            isSelected = false;
            targetSelectOffset = 0f;
        }
    }

    // ===== Shake =====
    public void Shake()
    {
        if (!isShaking)
            StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        float t = 0f;

        while (t < shakeDuration)
        {
            shakeOffset = Random.insideUnitCircle * shakeMagnitude;
            t += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector2.zero;
        isShaking = false;
    }

    // ===== 外部接口 =====

    /// <summary>
    /// 设置角色死亡
    /// </summary>
    public void SetDead()
    {
        isAlive = false;
        // 如果已经被选中，取消选中
        if (isSelected)
        {
            UISelectionManager.Instance.Unselect(this);
            isSelected = false;
        }

        // 可以加灰化效果或者不可点击提示
        // 例如：
        var img = GetComponent<UnityEngine.UI.Image>();
        if (img != null)
            img.color = Color.gray;

        targetSelectOffset = 0f; // 不再上移
    }

    public bool IsSelected => isSelected;
    public bool IsAlive => isAlive;
}
