using UnityEngine;
using System.Collections;

public class SimpleProjectile : MonoBehaviour
{
    [Header("飞行设置")]
    public Vector3 originPos;
    public Vector3 targetPos;
    public float flightTime = 2.0f;

    [Header("销毁设置")]
    public float destroyDelay = 0.5f; // 飞行结束后的额外等待时间

    private float startTime;
    private bool hasReachedTarget = false;

    void Start()
    {
        transform.position = originPos;
        
        if (targetPos != originPos)
        {
            transform.forward = (targetPos - originPos).normalized;
        }
        
        startTime = Time.time;
        
        LightingLine lightingLine = gameObject.GetComponent<LightingLine>();
        if (lightingLine != null)
        {
            lightingLine.StartPos = originPos;
            lightingLine.TargetPos = targetPos;
        }
        
        StartCoroutine(FlyToTarget());
    }

    private IEnumerator FlyToTarget()
    {
        // 飞行过程
        while (Time.time - startTime < flightTime)
        {
            float progress = (Time.time - startTime) / flightTime;
            transform.position = Vector3.Lerp(originPos, targetPos, progress);
            yield return null;
        }
        
        transform.position = targetPos;
        hasReachedTarget = true;
        
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
    
    public void SetFlightParameters(Vector3 origin, Vector3 target)
    {
        originPos = origin;
        targetPos = target;
        // flightTime = time;
        // destroyDelay = delay;
    }
    
    // void OnDrawGizmos()
    // {
    //     if (Application.isPlaying && !hasReachedTarget)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawLine(originPos, targetPos);
    //         Gizmos.DrawWireSphere(transform.position, 0.1f);
    //     }
    // }
}