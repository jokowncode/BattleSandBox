using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;

public class HeroLinkSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("交互设置")]
    public float doubleClickThreshold = 0.3f; // 双击时间阈值（秒）

    public int SlotIndex;
    [HideInInspector]public Hero Hero;
    
    [Header("颜色设置")]
    public Color normalColor = new Color(0.5f, 0.5f, 0.5f, 1f); // NORMAL
    public Color hoverColor = Color.green; // hIGHTLIGHT
    public Color selectedColor = Color.green; // SELECTED
    public Color pressedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // PRESSED
    
    [Header("事件回调")]
    public UnityEvent onLeftClick;
    public UnityEvent onRightClick;
    public UnityEvent onDoubleClick;
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;
    
    [Header("调试设置")]
    public bool enableDebugLogs = true; // 
    private float lastClickTime = 0f;
    private bool isPointerOver = false;
    private bool isSelected = false;
    private bool isPressed = false;
    private string objectName; // for debug
    
    private Image slotImage;
    private Material slotMaterial;
    private Color originalColor; // OriginCOlor

    
    private void Awake()
    {
        objectName = gameObject.name;
        slotImage = GetComponent<Image>();
        
        // 创建材质实例以避免修改原始材质
        if (slotImage.material != null)
        {
            slotImage.material = new Material(slotImage.material);
            slotMaterial = slotImage.material;
            originalColor = slotMaterial.GetColor("_FrameColor");
        }
        
        // 确保事件不为空
        if (onLeftClick == null) onLeftClick = new UnityEvent();
        if (onRightClick == null) onRightClick = new UnityEvent();
        if (onDoubleClick == null) onDoubleClick = new UnityEvent();
        if (onPointerEnter == null) onPointerEnter = new UnityEvent();
        if (onPointerExit == null) onPointerExit = new UnityEvent();
        
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Awake completed, events initialized");
        }
        
        UpdateAppearance();
    }

    private void Start()
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Start completed, ready for interactions");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isPointerOver)
            {
                isPressed = false;
                isSelected = false;
                UpdateAppearance();
            }
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Pointer entered");
        }
        
        UpdateAppearance();
        onPointerEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Pointer exited");
        }
        UpdateAppearance();
        onPointerExit.Invoke();
    }
    
    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     if (eventData.button == PointerEventData.InputButton.Left)
    //     {
    //         isPressed = true;
    //         UpdateAppearance();
    //         
    //         if (enableDebugLogs)
    //         {
    //             Debug.Log($"[HeroLinkClickableUI] {objectName} - Pointer down");
    //         }
    //     }
    // }
    //
    // public void OnPointerUp(PointerEventData eventData)
    // {
    //     if (eventData.button == PointerEventData.InputButton.Left)
    //     {
    //         isPressed = false;
    //         UpdateAppearance();
    //         
    //         if (enableDebugLogs)
    //         {
    //             Debug.Log($"[HeroLinkClickableUI] {objectName} - Pointer up");
    //         }
    //     }
    // }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Pointer clicked with button: {eventData.button}");
        }
        
        // 处理左键点击
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            float currentTime = Time.unscaledTime;
            float timeSinceLastClick = currentTime - lastClickTime;
            
            if (enableDebugLogs)
            {
                Debug.Log($"[HeroLinkClickableUI] {objectName} - Left click detected, time since last click: {timeSinceLastClick:F3}s");
            }
            
            // 检查是否为双击
            if (timeSinceLastClick < doubleClickThreshold)
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[HeroLinkClickableUI] {objectName} - Double click detected (within {timeSinceLastClick:F3}s)");
                }
                
                HandleDoubleLeftClick();
                // 触发双击事件
                onDoubleClick.Invoke();
                lastClickTime = 0; // 重置时间，避免连续三次点击触发两次双击
            }
            else
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[HeroLinkClickableUI] {objectName} - Potential single click, waiting for double click timeout");
                }

                
                HandleLeftClick();
                // 不是双击，记录点击时间并启动单击检测协程
                lastClickTime = currentTime;
                StartCoroutine(CheckForSingleClick());
            }
        }
        // 处理右键点击
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[HeroLinkClickableUI] {objectName} - Right click detected, invoking right click event");
            }
            
            onRightClick.Invoke();
        }
        // 处理中键点击（可选）
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[HeroLinkClickableUI] {objectName} - Middle click detected (no event bound)");
            }
        }
    }
    
    // 更新外观 based on current state
    private void UpdateAppearance()
    {
        if(SlotIndex==0)
            Debug.Log("Update Appearance");
        if (slotMaterial == null) return;

        Color targetColor = normalColor;
        float targetAlpha = 1f;

        if (isPressed)
        {
            targetColor = pressedColor;
            targetAlpha = pressedColor.a;
        }
        else if (isSelected)
        {
            targetColor = selectedColor;
        }
        else if (isPointerOver)
        {
            Debug.Log("Pointer over");
            targetColor = hoverColor;
        }
        else
        {
            targetColor = normalColor;
        }

        // 设置材质颜色
        slotMaterial.SetColor("_FrameColor", targetColor);
        
        // 设置UI图像的透明度
        if (slotImage != null)
        {
            Color imageColor = slotImage.color;
            slotImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, targetAlpha);
        }
    }

    public void HandleLeftClick()
    {
        isPressed = false;
        isSelected = true;
        UpdateAppearance();
        HeroLinkUI.Instance.SelectSlot(slotIndex:SlotIndex);
        //Debug.Log("左键");
    }

    public void HandleDoubleLeftClick()
    {
        isPressed = false;
        isSelected = true;
        UpdateAppearance();
        this.GetComponent<Image>().sprite = null;
        this.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        if(Hero!=null)
        {
            HeroLinkUI.Instance.RemoveHeroFromSlot(Hero);
            Hero = null;
        }
        //Debug.Log("双击左键");
    }
    
    private IEnumerator CheckForSingleClick()
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Starting single click check coroutine");
        }
        
        yield return new WaitForSecondsRealtime(doubleClickThreshold);
        
        if (Time.unscaledTime - lastClickTime >= doubleClickThreshold)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[HeroLinkClickableUI] {objectName} - Single click confirmed, invoking left click event");
            }
            
            onLeftClick.Invoke();
        }
        else
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[HeroLinkClickableUI] {objectName} - Single click check cancelled (double click detected instead)");
            }
        }
    }
    
    // 提供给外部调用的方法，用于设置回调函数
    public void SetLeftClickCallback(UnityAction callback)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Setting left click callback");
        }
        
        onLeftClick.RemoveAllListeners();
        onLeftClick.AddListener(callback);
    }
    
    public void SetRightClickCallback(UnityAction callback)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Setting right click callback");
        }
        
        onRightClick.RemoveAllListeners();
        onRightClick.AddListener(callback);
    }
    
    public void SetDoubleClickCallback(UnityAction callback)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Setting double click callback");
        }
        
        onDoubleClick.RemoveAllListeners();
        onDoubleClick.AddListener(callback);
    }
    
    public void SetPointerEnterCallback(UnityAction callback)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Setting pointer enter callback");
        }
        
        onPointerEnter.RemoveAllListeners();
        onPointerEnter.AddListener(callback);
    }
    
    public void SetPointerExitCallback(UnityAction callback)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - Setting pointer exit callback");
        }
        
        onPointerExit.RemoveAllListeners();
        onPointerExit.AddListener(callback);
    }
    
    // 检查鼠标是否悬停在UI上
    public bool IsPointerOver()
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - IsPointerOver called, returning: {isPointerOver}");
        }
        
        return isPointerOver;
    }
    
    // 设置调试日志启用状态
    public void SetDebugLogsEnabled(bool enabled)
    {
        enableDebugLogs = enabled;
        Debug.Log($"[HeroLinkClickableUI] {objectName} - Debug logs {(enabled ? "enabled" : "disabled")}");
    }
    
    // 在禁用时重置状态
    private void OnDisable()
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - OnDisable called, resetting state");
        }
        
        isPointerOver = false;
        lastClickTime = 0;
        StopAllCoroutines();
    }
    
    // 在销毁时清理
    private void OnDestroy()
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[HeroLinkClickableUI] {objectName} - OnDestroy called, cleaning up");
        }
        
        // 移除所有事件监听器以避免内存泄漏
        onLeftClick.RemoveAllListeners();
        onRightClick.RemoveAllListeners();
        onDoubleClick.RemoveAllListeners();
        onPointerEnter.RemoveAllListeners();
        onPointerExit.RemoveAllListeners();
    }
}