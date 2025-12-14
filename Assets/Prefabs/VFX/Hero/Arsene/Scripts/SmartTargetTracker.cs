using UnityEngine;
using System.Collections.Generic;

public class SmartTargetTracker : MonoBehaviour
{
    public enum TargetType
    {
        Enemy,
        Hero
    }

    [Header("技能目标类型")]
    public TargetType targetType = TargetType.Enemy;

    [Header("移动设置")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float sharpTurnMultiplier = 3f;

    [Header("转弯逻辑")]
    public float sharpTurnAngle = 60f;
    public float smoothTurnAngle = 30f;
    public float predictionDistance = 3f;

    private Transform currentTarget;
    private Rigidbody rb;

    [Header("调试")]
    public bool showDebugGizmos = true;
    public Color detectionRangeColor = Color.yellow;
    public Color targetLineColor = Color.red;
    public Color predictionLineColor = Color.blue;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }

        // 请求目标
        RequestTarget();
    }

    void Update()
    {
        if (currentTarget == null)
        {
            // 如果没有目标，尝试请求目标
            RequestTarget();
            return;
        }

        MoveTowardsTarget();
    }

    void RequestTarget()
    {
        // 根据类型向目标管理器请求目标
        if (targetType == TargetType.Enemy)
        {
            currentTarget = TargetManager.Instance.RequestEnemyTarget(GetInstanceID());
        }
        else
        {
            currentTarget = TargetManager.Instance.RequestAllyTarget(GetInstanceID());
        }

        // 如果请求到目标，开始移动
        if (currentTarget != null)
        {
            // 可以在这里处理一些事情，比如开始移动
        }
    }

    void MoveTowardsTarget()
    {
        if (currentTarget == null) return;

        Vector3 directionToTarget = (currentTarget.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        // 计算当前前进方向与目标方向的角度差
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // 根据角度差决定旋转速度
        float currentRotationSpeed = rotationSpeed;
        if (angleToTarget > sharpTurnAngle)
        {
            currentRotationSpeed *= sharpTurnMultiplier;
        }
        else if (angleToTarget > smoothTurnAngle)
        {
            currentRotationSpeed *= (sharpTurnMultiplier + 1f) / 2f;
        }

        // 使用预测点来平滑转弯（如果目标有Rigidbody）
        Vector3 predictedPosition = currentTarget.position;
        Rigidbody targetRb = currentTarget.GetComponent<Rigidbody>();
        if (targetRb != null && predictionDistance > 0)
        {
            predictedPosition += targetRb.velocity.normalized * predictionDistance;
        }

        Vector3 directionToPredicted = (predictedPosition - transform.position).normalized;

        // 计算朝向预测点的旋转
        Quaternion targetRotation = Quaternion.LookRotation(directionToPredicted);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);

        // 移动前进
        Vector3 moveDirection = transform.forward;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void OnDestroy()
    {
        // 当技能物体销毁时，释放目标
        if (currentTarget != null)
        {
            if (targetType == TargetType.Enemy)
            {
                TargetManager.Instance.ReleaseEnemyTarget(currentTarget, GetInstanceID());
            }
            else
            {
                TargetManager.Instance.ReleaseAllyTarget(currentTarget, GetInstanceID());
            }
        }
    }

    // 调试可视化
    void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;

        if (currentTarget != null)
        {
            // 绘制到目标的线
            Gizmos.color = targetLineColor;
            Gizmos.DrawLine(transform.position, currentTarget.position);

            // 绘制预测点
            Vector3 predictedPosition = currentTarget.position;
            Rigidbody targetRb = currentTarget.GetComponent<Rigidbody>();
            if (targetRb != null && predictionDistance > 0)
            {
                predictedPosition += targetRb.velocity.normalized * predictionDistance;
            }

            Gizmos.color = predictionLineColor;
            Gizmos.DrawLine(transform.position, predictedPosition);
            Gizmos.DrawWireSphere(predictedPosition, 0.5f);

            // 绘制前进方向
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * 3f);
        }
    }
}