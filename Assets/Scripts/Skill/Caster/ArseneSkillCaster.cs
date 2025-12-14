using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class ArseneSkillCaster : SkillCaster 
{
    [Header("Skill Delivery Settings")]
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 10f; // 技能攻击范围
    [SerializeField] private float overshootDistance = 3f;
    [SerializeField] private float destroyDelay = 1f;
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Direction Settings")]
    [SerializeField] private bool useForwardDirection = true; // 使用transform.forward方向
    [SerializeField] private Vector3 customDirection = Vector3.right; // 或自定义方向

    protected void InitializeSkillDelivery() {
        if (skillPrefab == null)
        {
            Debug.LogError("Skill prefab is not assigned!");
            return;
        }
        
        GameObject skillInstance = Instantiate(skillPrefab, transform.position, Quaternion.identity);
        
        // 确定发射方向
        Vector3 direction = GetSkillDirection();
        skillInstance.transform.rotation = Quaternion.LookRotation(direction);
        
        StartCoroutine(MoveSkillInstanceForward(skillInstance, direction));
    }
    
    private Vector3 GetSkillDirection()
    {
        if (useForwardDirection)
        {
            return transform.forward;
        }
        else
        {
            // 将自定义方向转换为世界空间方向
            return transform.TransformDirection(customDirection.normalized);
        }
    }
    
    private IEnumerator MoveSkillInstanceForward(GameObject skillInstance, Vector3 direction)
    {
        Vector3 startPosition = skillInstance.transform.position;
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
        
        while (journey < distanceToTarget)
        {
            journey += moveSpeed * Time.deltaTime;
            float percent = Mathf.Clamp01(journey / distanceToTarget);
            float curvePercent = moveCurve.Evaluate(percent);
            
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, curvePercent);
            newPosition.y = fixedY;
            
            skillInstance.transform.position = newPosition;
            yield return null;
        }
        
        skillInstance.transform.position = targetPosition;
        
        // 第二阶段：继续向前移动（过冲效果）
        journey = 0f;
        float distanceToOvershoot = overshootDistance;
        
        while (journey < distanceToOvershoot)
        {
            journey += moveSpeed * Time.deltaTime;
            float percent = Mathf.Clamp01(journey / distanceToOvershoot);
            float curvePercent = moveCurve.Evaluate(percent);
            
            Vector3 newPosition = Vector3.Lerp(targetPosition, overshootPosition, curvePercent);
            newPosition.y = fixedY;
            
            skillInstance.transform.position = newPosition;
            yield return null;
        }
        
        skillInstance.transform.position = overshootPosition;
        
        // 延迟销毁
        yield return new WaitForSeconds(destroyDelay);
        
        if (skillInstance != null)
            Destroy(skillInstance);
    }
    
    // 可选：添加碰撞检测来处理命中
    private void OnTriggerEnter(Collider other)
    {
        // 这里可以添加命中敌人的逻辑
        // 例如：if (other.CompareTag("Enemy")) { /* 处理伤害 */ }
    }
    
    public float CalculateYRotationToTarget(Vector3 source, Vector3 target)
    {
        Vector3 direction = new Vector3(
            target.x - source.x,
            0,
            target.z - source.z
        );
        
        if (direction.sqrMagnitude < 0.0001f) return 0f;
        
        direction.Normalize();
        
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        
        // 转换为0-360度
        if (angle < 0) angle += 360f;
        
        return angle;
    }
    
    protected override void Cast(Transform attackTarget){
        // 忽略传入的目标，只朝前方发射
        InitializeSkillDelivery();
    }
    
    // 可选：添加一个无参数的Cast方法重载
    public void CastForward()
    {
        InitializeSkillDelivery();
    }
}