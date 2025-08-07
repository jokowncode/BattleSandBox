using UnityEngine;

public class UIShaker : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 10f;

    private RectTransform rectTransform;
    private Vector3 originalPos;
    private bool isShaking = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
    }

    public void SetOriginalPos(Vector2 pos)
    {
        originalPos = pos;
    }
    
    public void Shake()
    {
        if (!isShaking)
            StartCoroutine(ShakeCoroutine());
    }

    private System.Collections.IEnumerator ShakeCoroutine()
    {
        isShaking = true;
        float timer = 0f;

        while (timer < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;
            rectTransform.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            timer += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
        isShaking = false;
    }
}