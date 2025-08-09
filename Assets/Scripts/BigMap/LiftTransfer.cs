using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 传送系统：点击按钮后将玩家传送到指定位置，并处理UI的淡入淡出。
/// </summary>
public class LiftTransfer : MonoBehaviour
{
    [Header("传送设置")]
    [Tooltip("目标传送位置")]
    public Transform targetPosition;
    
    [Tooltip("传送按钮")]
    public Button transferButton;
    
    [Header("UI及淡入淡出设置")]
    public CanvasGroup HUDCanvasGroup;

    [Tooltip("用于黑屏效果的UI Image面板")]
    public Image fadePanel;
    
    [Tooltip("淡入淡出时间（秒）")]
    public float fadeTime = 1.0f;
    
    [Tooltip("传送后的延迟时间（秒）")]
    public float transferDelay = 0.5f;

    private bool isTransferring = false;
    private GameObject player;
    private CanvasGroup mainCanvasGroup;
    private CanvasGroup secondaryCanvasGroup;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player == null) Debug.LogError("场景中未找到带有'Player'标签的对象！", this);
        if (targetPosition == null) Debug.LogError("请在Inspector中设置'targetPosition'。", this);
        if (transferButton == null) Debug.LogError("请在Inspector中设置'transferButton'。", this);
        if (fadePanel == null) Debug.LogError("请在Inspector中设置'fadePanel'。", this);

        /*if (panelToFade == null) 
        {
            Debug.LogError("请在Inspector中设置'panelToFade'。", this);
        }
        else
        {
            mainCanvasGroup = panelToFade.GetComponent<CanvasGroup>();
            if (mainCanvasGroup == null)
            {
                Debug.LogError($"'panelToFade' ({panelToFade.name}) 上没有找到CanvasGroup组件，请为其添加一个。", this);
            }
        }

        if (secondaryPanelToFade != null)
        {
            secondaryCanvasGroup = secondaryPanelToFade.GetComponent<CanvasGroup>();
            if (secondaryCanvasGroup == null)
            {
                Debug.LogError($"'secondaryPanelToFade' ({secondaryPanelToFade.name}) 上没有找到CanvasGroup组件，请为其添加一个。", this);
            }
        }*/
        
        SetupButtonEvent();
        InitializeUI();
    }
    
    /// <summary>
    /// 初始化UI状态。
    /// </summary>
    private void InitializeUI()
    {
        if (fadePanel != null)
        {
            Color color = fadePanel.color;
            color.a = 0;
            fadePanel.color = color;
            fadePanel.raycastTarget = false;
        }
        if (mainCanvasGroup != null)
        {
            mainCanvasGroup.alpha = 1;
        }
        if (secondaryCanvasGroup != null)
        {
            secondaryCanvasGroup.alpha = 1;
        }
    }

    /// <summary>
    /// 自动为按钮设置点击事件监听器。
    /// </summary>
    private void SetupButtonEvent()
    {
        if (transferButton != null)
        {
            transferButton.onClick.RemoveAllListeners();
            transferButton.onClick.AddListener(OnTransferButtonClicked);
        }
    }

    /// <summary>
    /// 当传送按钮被点击时调用。
    /// </summary>
    public void OnTransferButtonClicked()
    {
        if (!isTransferring && player != null && targetPosition != null && mainCanvasGroup != null)
        {
            StartCoroutine(TransferPlayer());
        }
    }
    
    /// <summary>
    /// 手动触发传送的公共方法。
    /// </summary>
    public void TriggerTransfer()
    {
        OnTransferButtonClicked();
    }

    /// <summary>
    /// 执行传送过程的协程。
    /// </summary>
    private IEnumerator TransferPlayer()
    {
        isTransferring = true;
        
        if (transferButton != null) transferButton.interactable = false;

        yield return StartCoroutine(FadeCanvas(0f, fadeTime));
        yield return StartCoroutine(FadeScreen(1f, 0.2f));

        player.transform.position = targetPosition.position;
        player.transform.rotation = targetPosition.rotation;
        
        yield return new WaitForSeconds(transferDelay);
        
        yield return StartCoroutine(FadeScreen(0f, 0.2f));
        yield return StartCoroutine(FadeCanvas(1f, fadeTime));
        
        if (transferButton != null) transferButton.interactable = true;
        
        isTransferring = false;
    }

    /// <summary>
    /// 控制一个或两个CanvasGroup淡入淡出的协程。
    /// </summary>
    private IEnumerator FadeCanvas(float targetAlpha, float duration)
    {
        if (mainCanvasGroup == null) yield break;

        float startAlpha = mainCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            
            mainCanvasGroup.alpha = currentAlpha;
            if (secondaryCanvasGroup != null)
            {
                secondaryCanvasGroup.alpha = currentAlpha;
            }
            yield return null;
        }

        mainCanvasGroup.alpha = targetAlpha;
        if (secondaryCanvasGroup != null)
        {
            secondaryCanvasGroup.alpha = targetAlpha;
        }
    }

    /// <summary>
    /// 控制黑屏效果的协程。
    /// </summary>
    private IEnumerator FadeScreen(float targetAlpha, float duration)
    {
        if (fadePanel == null) yield break;

        fadePanel.raycastTarget = true;

        float startAlpha = fadePanel.color.a;
        float elapsedTime = 0f;
        Color color = fadePanel.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            fadePanel.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadePanel.color = color;

        if (targetAlpha == 0)
        {
            fadePanel.raycastTarget = false;
        }
    }
}
