using UnityEngine;

public class UIPopping : MonoBehaviour
{
    [Tooltip("控制跳动的轴向")]
    public Vector3 moveAxis = Vector3.up;

    [Tooltip("跳动的速度")]
    public float speed = 2f;

    [Tooltip("沿轴向运动的最小偏移量")]
    public float minOffset = -0.25f;

    [Tooltip("沿轴向运动的最大偏移量")]
    public float maxOffset = 0.25f;

    private Vector3 startPosition;
    private float initialOffset;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 将Sin函数的-1到1范围映射到minOffset和maxOffset之间
        float range = maxOffset - minOffset;
        float offset = minOffset + (Mathf.Sin(Time.time * speed) + 1f) / 2f * range;
        transform.position = startPosition + moveAxis.normalized * offset;
    }
}
