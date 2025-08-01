using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个先进的灯光控制器，允许在同一个列表中为每个灯光单独指定其行为模式（普通或警报）。
/// </summary>
public class AdvancedLightController : MonoBehaviour
{
    // 定义灯光的两种模式
    public enum LightMode
    {
        General,
        Alarm
    }

    // 一个自定义类，用于在Inspector中将灯光和其模式配对
    [System.Serializable]
    public class ControlledLight
    {
        public Light lightSource;
        public LightMode mode = LightMode.General;
    }

    [Header("灯光列表和通用设置")]
    [Tooltip("在此列表中配置所有受控灯光及其模式")]
    [SerializeField]
    private List<ControlledLight> controlledLights;

    [Tooltip("灯光强度变化的过渡时间")]
    [SerializeField]
    private float transitionDuration = 1.0f;

    [Tooltip("玩家离开时，所有灯光恢复到的待机强度")]
    [SerializeField]
    private float inactiveIntensity = 0.1f;

    [Header("普通灯光设置")]
    [Tooltip("玩家进入时普通灯光的强度")]
    [SerializeField]
    private float generalActiveIntensity = 2f;

    [Header("警报灯设置")]
    [Tooltip("警报闪烁时的颜色")]
    [SerializeField]
    private Color alarmColor = Color.red;

    [Tooltip("每秒闪烁的次数")]
    [SerializeField]
    private float alarmFlashFrequency = 2f;

    [Tooltip("警报灯亮起时的强度")]
    [SerializeField]
    private float alarmHighIntensity = 5f;

    // --- 私有变量 ---
    private Dictionary<Light, Coroutine> runningCoroutines = new Dictionary<Light, Coroutine>();
    private Dictionary<Light, Color> originalLightColors = new Dictionary<Light, Color>();

    private void Awake()
    {
        // 初始化所有灯光
        foreach (var controlledLight in controlledLights)
        {
            Light light = controlledLight.lightSource;
            if (light != null)
            {
                originalLightColors[light] = light.color;
                light.intensity = inactiveIntensity;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var controlledLight in controlledLights)
            {
                Light light = controlledLight.lightSource;
                if (light == null) continue;

                // 根据每个灯光自己的模式来决定行为
                switch (controlledLight.mode)
                {
                    case LightMode.General:
                        StopAndRunCoroutine(light, FadeIntensity(light, generalActiveIntensity, transitionDuration));
                        break;
                    case LightMode.Alarm:
                        StopAndRunCoroutine(light, FlashAlarm(light));
                        break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 玩家离开，所有灯光统一恢复到待机状态
            foreach (var controlledLight in controlledLights)
            {
                Light light = controlledLight.lightSource;
                if (light == null) continue;

                // 恢复原始颜色（对警报灯尤其重要）
                if (originalLightColors.ContainsKey(light))
                {
                    light.color = originalLightColors[light];
                }

                // 优化：对于警报灯，先停止其闪烁，然后从0强度开始平滑过渡，避免卡顿感
                if(controlledLight.mode == LightMode.Alarm)
                {
                    if (runningCoroutines.ContainsKey(light) && runningCoroutines[light] != null)
                    {
                        StopCoroutine(runningCoroutines[light]);
                        runningCoroutines.Remove(light);
                    }
                    light.intensity = 0; // 立即设置为0，以此为起点开始过渡
                }
                
                StopAndRunCoroutine(light, FadeIntensity(light, inactiveIntensity, transitionDuration));
            }
        }
    }

    private void StopAndRunCoroutine(Light light, IEnumerator newCoroutine)
    {
        if (runningCoroutines.ContainsKey(light) && runningCoroutines[light] != null)
        {
            StopCoroutine(runningCoroutines[light]);
        }
        runningCoroutines[light] = StartCoroutine(newCoroutine);
    }

    private IEnumerator FadeIntensity(Light light, float targetIntensity, float duration)
    {
        // 如果持续时间为0或负数，直接设置值并退出
        if (duration <= 0)
        {
            if (light != null) light.intensity = targetIntensity;
            yield break;
        }

        float startIntensity = light.intensity;
        float time = 0;

        while (time < duration)
        {
            if (light == null) yield break; // 如果灯在中途被销毁，安全退出
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        if (light != null)
        {
            light.intensity = targetIntensity;
        }
    }

    private IEnumerator FlashAlarm(Light light)
    {
        light.color = alarmColor;
        float flashDuration = 1f / alarmFlashFrequency / 2f;

        while (true)
        {
            // 在亮暗之间平滑过渡
            yield return StartCoroutine(FadeIntensity(light, alarmHighIntensity, flashDuration));
            // 在协程返回后，检查灯是否还存在
            if (light == null) yield break;
            yield return StartCoroutine(FadeIntensity(light, 0, flashDuration));
            if (light == null) yield break;
        }
    }
}
