using UnityEngine;
using System.Collections;

public class UIShaker : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 10f;

    Vector2 shakeOffset;
    bool isShaking;

    /// <summary>
    /// 由 UISelection 读取
    /// </summary>
    public Vector2 CurrentOffset => shakeOffset;

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
}