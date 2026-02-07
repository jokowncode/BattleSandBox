using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UISelectableShaker : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("Selection")]
    public float hoverOffsetY = 40f;
    public float moveSpeed = 12f;

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

    public bool HasTactic { get; private set; } = false;
    
    [HideInInspector] public Hero CurrentHero;

    private Vector2 InitialAnchorPosition;
    
    void Awake() {
        rect = GetComponent<RectTransform>();
    }

    public void SetInitialAnchorPosition() {
        this.InitialAnchorPosition = rect.anchoredPosition;
    }

    void LateUpdate() {
        currentSelectOffset = Mathf.Lerp(currentSelectOffset, targetSelectOffset, Time.deltaTime * moveSpeed);
        rect.anchoredPosition = this.InitialAnchorPosition + Vector2.up * currentSelectOffset + shakeOffset;
    }

    // ===== Selection =====
    public void OnPointerEnter(PointerEventData eventData) {
        if (!isAlive || isSelected) return;
        if (!BattleUIManager.Instance.heroPortraitUI.HeroEnergyIsFull(this.CurrentHero.Name)) return;
        if (!HasTactic && UISelectionManager.Instance.SelectedSize != 0) return;
        targetSelectOffset = hoverOffsetY;
    }

    public void HasEntanglement() {
        this.HasTactic = true;
        targetSelectOffset = hoverOffsetY / 2.0f;
    }

    public void GoDown(bool cancelHasTactic) {
        if (cancelHasTactic) {
            this.HasTactic = false;
        }
        this.isSelected = false;
        if (this.HasTactic) {
            HasEntanglement();
        } else {
            targetSelectOffset = 0.0f;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (isSelected) return;
        GoDown(false);
    }

    public void BeSelected() {
        if (!isAlive) return;

        if (!isSelected) {
            // 尝试选中
            if (UISelectionManager.Instance.TrySelect(this)) {
                isSelected = true;
                HasTactic = false;
                targetSelectOffset = hoverOffsetY;
            }
        } else {
            // 取消选中
            UISelectionManager.Instance.Unselect(this);
            GoDown(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        BeSelected();
    }

    // ===== Shake =====
    public void Shake() {
        if (!isShaking)
            StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine() {
        isShaking = true;
        float t = 0f;

        while (t < shakeDuration) {
            shakeOffset = Random.Range(-1f, 1f) * shakeMagnitude * Vector2.right;
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
        if (TryGetComponent(out Image img))
            img.color = Color.gray;

        GoDown(true); // 不再上移
    }

    public bool IsSelected => isSelected;
    public bool IsAlive => isAlive;
}
