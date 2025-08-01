using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    // 将房间内的所有点光源拖到这个列表里
    [SerializeField]
    private List<Light> roomLights;

    // 玩家进入时的灯光强度
    [SerializeField]
    private float activeIntensity = 2f;

    // 玩家离开时的灯光强度
    [SerializeField]
    private float inactiveIntensity = 0.1f;

    // 灯光强度变化的过渡时间
    [SerializeField]
    private float transitionDuration = 1.0f;

    // 用来存储每个灯光对应的正在运行的协程，方便管理
    private Dictionary<Light, Coroutine> lightCoroutines = new Dictionary<Light, Coroutine>();

    private void Awake()
    {
        // 游戏开始时，先将所有灯光设置为离开时的强度
        foreach (var light in roomLights)
        {
            if (light != null)
            {
                light.intensity = inactiveIntensity;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查进入碰撞盒的是否是玩家
        if (other.CompareTag("Player"))
        {
            // 为每个灯光启动一个“渐亮”的协程
            foreach (var light in roomLights)
            {
                if (light != null)
                {
                    // 如果这个灯光之前有正在运行的协程，先停止它
                    if (lightCoroutines.ContainsKey(light))
                    {
                        StopCoroutine(lightCoroutines[light]);
                    }
                    // 启动新的协程，并将其存入字典中
                    Coroutine newCoroutine = StartCoroutine(FadeLight(light, activeIntensity));
                    lightCoroutines[light] = newCoroutine;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 检查离开碰撞盒的是否是玩家
        if (other.CompareTag("Player"))
        {
            // 为每个灯光启动一个“渐暗”的协程
            foreach (var light in roomLights)
            {
                if (light != null)
                {
                    if (lightCoroutines.ContainsKey(light))
                    {
                        StopCoroutine(lightCoroutines[light]);
                    }
                    Coroutine newCoroutine = StartCoroutine(FadeLight(light, inactiveIntensity));
                    lightCoroutines[light] = newCoroutine;
                }
            }
        }
    }

    /// <summary>
    /// 一个协程，用来在指定时间内平滑地改变灯光强度
    /// </summary>
    /// <param name="light">要改变的灯光</param>
    /// <param name="targetIntensity">目标强度</param>
    /// <returns></returns>
    private IEnumerator FadeLight(Light light, float targetIntensity)
    {
        float startIntensity = light.intensity;
        float time = 0;

        while (time < transitionDuration)
        {
            // 使用 Mathf.Lerp 进行线性插值，计算当前帧的灯光强度
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / transitionDuration);
            // 时间累加
            time += Time.deltaTime;
            // 等待下一帧
            yield return null;
        }

        // 循环结束后，确保灯光强度精确地设置为目标值
        light.intensity = targetIntensity;

        // 协程运行完毕，从字典中移除记录
        lightCoroutines.Remove(light);
    }
}
