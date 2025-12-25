
using System;
using System.Collections;
using UnityEngine;

public class ArseneSkillGroundTrail : MonoBehaviour {
    [Header("Skill Delivery Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float attackRange = 10f; // 技能攻击范围
    [SerializeField] private float overshootDistance = 3f;
    [SerializeField] private float destroyDelay = 0.5f;
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Direction Settings")]
    [SerializeField] private bool useForwardDirection = false; // 使用transform.forward方向
    [SerializeField] private Vector3 customDirection = Vector3.right; // 或自定义方向

    private void Start() {
        // 确定发射方向
        Vector3 direction = GetSkillDirection();
        this.transform.rotation = Quaternion.LookRotation(direction);
        StartCoroutine(MoveSkillInstanceForward(direction));
    }
    
    private IEnumerator MoveSkillInstanceForward(Vector3 direction) {
        Vector3 startPosition = this.transform.position;
        float fixedY = startPosition.y;
        
        // 计算目标位置（前方攻击范围处）
        Vector3 targetPosition = startPosition + direction * attackRange;
        targetPosition.y = fixedY;
        
        // 计算过冲位置
        Vector3 overshootPosition = targetPosition + direction * overshootDistance;
        overshootPosition.y = fixedY;
        
        // 第一阶段：移动到攻击范围点
        float journey = 0f;
        float distanceToTarget = Vector3.Distance(startPosition, targetPosition);
        
        while (journey < distanceToTarget) {
            journey += moveSpeed * Time.deltaTime;
            float percent = Mathf.Clamp01(journey / distanceToTarget);
            float curvePercent = moveCurve.Evaluate(percent);
            
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, curvePercent);
            newPosition.y = fixedY;
            
            this.transform.position = newPosition;
            yield return null;
        }
        
        this.transform.position = targetPosition;
        
        // 第二阶段：继续向前移动（过冲效果）
        journey = 0f;
        float distanceToOvershoot = overshootDistance;
        
        while (journey < distanceToOvershoot) {
            journey += moveSpeed * Time.deltaTime;
            float percent = Mathf.Clamp01(journey / distanceToOvershoot);
            float curvePercent = moveCurve.Evaluate(percent);
            
            Vector3 newPosition = Vector3.Lerp(targetPosition, overshootPosition, curvePercent);
            newPosition.y = fixedY;
            
            this.transform.position = newPosition;
            yield return null;
        }
        
        this.transform.position = overshootPosition;
        
        // 延迟销毁
        yield return new WaitForSeconds(destroyDelay);
        
        Destroy(this.gameObject);
    }

    private Vector3 GetSkillDirection() {
        if (useForwardDirection) {
            return transform.forward;
        } else {
            // 将自定义方向转换为世界空间方向
            return transform.TransformDirection(customDirection.normalized);
        }
    }
}


