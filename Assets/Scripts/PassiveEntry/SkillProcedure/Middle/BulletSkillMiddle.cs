
using UnityEngine;

public class BulletSkillMiddle : SkillMiddle
{
    public GameObject fireTrailPrefab;
    
    public override void AdditionalProcedure(GameObject target)
    {
        //Debug.Log();
        // 实例化粒子特效预制体
        GameObject trailInstance = GameObject.Instantiate(fireTrailPrefab, target.transform.position, Quaternion.identity);

        // 将 trailInstance 设置为 target 的子对象，使其跟随移动
        trailInstance.transform.SetParent(target.transform, worldPositionStays: true);

        // 获取其中的粒子系统
        ParticleSystem ps = trailInstance.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            // 启动粒子系统（通常 prefab 已经配置好自动播放）
            ps.Play();

            // 自动销毁粒子系统对象，当播放完毕后
            GameObject.Destroy(trailInstance, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Debug.LogWarning("预制体中没有找到 ParticleSystem 组件");
        }
    }
}

