using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个简单的、可通过外部脚本调用的警报灯控制器。
/// 包含一个选项，可让警报在游戏开始时自动启动。
/// </summary>
public class SimpleAlarmController : MonoBehaviour
{
    [Header("通用设置")]
    [Tooltip("如果勾选，警报将在游戏开始时自动启动")]
    [SerializeField]
    private bool startOnAwake = false;

    [Header("警报灯设置")]
    [Tooltip("需要作为警报灯闪烁的灯光列表")]
    [SerializeField]
    private List<Light> alarmLights;

    [Tooltip("警报闪烁时的颜色")]
    [SerializeField]
    private Color alarmColor = Color.red;

    [Tooltip("每秒闪烁的次数")]
    [SerializeField]
    private float flashFrequency = 2f;

    [Tooltip("警报闪烁时的最高强度")]
    [SerializeField]
    private float highIntensity = 5f;
    
    [Tooltip("警报闪烁时的最低强度（大于0则不会完全熄灭）")]
    [SerializeField]
    private float lowIntensity = 0.5f;

    // 用于存储每个灯光的原始状态
    private Dictionary<Light, (Color originalColor, float originalIntensity)> originalLightStates = new Dictionary<Light, (Color, float)>();
    // 用于存储正在运行的协程
    private Dictionary<Light, Coroutine> runningCoroutines = new Dictionary<Light, Coroutine>();

    private void Awake()
    {
        foreach (var light in alarmLights)
        {
            if (light != null)
            {
                originalLightStates[light] = (light.color, light.intensity);
            }
        }
    }

    private void Start()
    {
        if (startOnAwake)
        {
            StartAlarm();
        }
    }

    /// <summary>
    /// 公开方法：启动警报。
    /// </summary>
    public void StartAlarm()
    {
        foreach (var light in alarmLights)
        {
            if (light != null)
            {
                // 先停止任何可能已在运行的旧协程
                if (runningCoroutines.ContainsKey(light) && runningCoroutines[light] != null)
                {
                    StopCoroutine(runningCoroutines[light]);
                }
                // 启动新协程并保存其引用
                runningCoroutines[light] = StartCoroutine(FlashAlarm(light));
            }
        }
    }

    /// <summary>
    /// 公开方法：停止警报并恢复初始状态。
    /// </summary>
    public void StopAlarm()
    {
        foreach (var light in alarmLights)
        {
            if (light != null)
            {
                // 停止正在运行的协程
                if (runningCoroutines.ContainsKey(light) && runningCoroutines[light] != null)
                {
                    StopCoroutine(runningCoroutines[light]);
                    runningCoroutines.Remove(light);
                }
                
                // 恢复灯光的初始状态
                if (originalLightStates.ContainsKey(light))
                {
                    var originalState = originalLightStates[light];
                    light.color = originalState.originalColor;
                    light.intensity = originalState.originalIntensity;
                }
            }
        }
    }

    /// <summary>
    /// 主闪烁协程。
    /// </summary>
    private IEnumerator FlashAlarm(Light light)
    {
        if (flashFrequency <= 0) yield break;

        float flashDuration = 1f / flashFrequency / 2f;
        light.color = alarmColor;

        while (true)
        {
            // 这是最初可以工作的、正确的写法
            yield return StartCoroutine(FadeIntensity(light, highIntensity, flashDuration));
            if (light == null) yield break;
            yield return StartCoroutine(FadeIntensity(light, lowIntensity, flashDuration));
            if (light == null) yield break;
        }
    }

    /// <summary>
    /// 控制强度渐变的子协程。
    /// </summary>
    private IEnumerator FadeIntensity(Light light, float targetIntensity, float duration)
    {
        if (duration <= 0)
        {
            if(light != null) light.intensity = targetIntensity;
            yield break;
        }
        
        float startIntensity = light.intensity;
        float timer = 0f;
        
        while(timer < duration)
        {
            if(light == null) yield break;
            timer += Time.deltaTime;
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, timer / duration);
            yield return null;
        }

        if(light != null) light.intensity = targetIntensity;
    }
}
