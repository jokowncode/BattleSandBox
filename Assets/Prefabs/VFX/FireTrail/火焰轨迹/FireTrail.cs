using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireTrail : MonoBehaviour {
    public float spawnDistance = 1.0f; // 移动多远生成一个Trail
    public FireTrailSingleVFX fireTrailPrefab; // 带 VFXAutoStop 的 Trail 预制体
    public float movementThreshold = 0.001f; // 判断是否停下的阈值
    public float maxOffset = 0.3f;

    private Vector3 lastSpawnPosition; // 上次生成 Trail 的位置
    private Vector3 lastPosition;      // 上一帧位置
    private bool stop;                  // 是否停止移动

    private float Damage;
    
    void Start()
    {
        lastSpawnPosition = transform.position;
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        CheckIfStopped();
        TrySpawnTrail();
    }

    // 检测是否停下
    private void CheckIfStopped()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        stop = distance < movementThreshold;
        lastPosition = transform.position;
    }

    // 根据移动距离生成 Trail
    private void TrySpawnTrail()
    {
        if (!stop)
        {
            float distanceFromLastTrail = Vector3.Distance(transform.position, lastSpawnPosition);

            if (distanceFromLastTrail >= spawnDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -Vector3.up, out hit))
                {
                    Vector3 spawnPos = hit.point + Vector3.up * 0.1f;
                    float randomOffset = Random.Range(-maxOffset, maxOffset);
                    Vector3 offsetPos = spawnPos + transform.right * randomOffset;
                    SpawnVFXAt(offsetPos);

                    lastSpawnPosition = transform.position; // 更新上一次生成的位置
                }
            }
        }
    }

    public void SetDamage(float damage){
        this.Damage = damage;
    }
    
    // 生成 Trail 预制体
    private void SpawnVFXAt(Vector3 position){
        FireTrailSingleVFX singleFire = Instantiate(fireTrailPrefab, position, Quaternion.identity);
        singleFire.SetDamage(this.Damage);
    }
}