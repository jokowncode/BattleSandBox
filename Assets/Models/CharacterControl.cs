using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;  // 移动速度
    
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // 获取Rigidbody组件
        rb = GetComponent<Rigidbody>();
        
        // 如果没有Rigidbody组件，添加一个
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        // 锁定Y和Z轴旋转，防止角色倾倒
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }
    
    void HandleMovement()
    {
        // 获取水平输入 (A键为-1, D键为1)
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // 计算移动向量 (在X轴上移动，实现左右移动)
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * moveSpeed;
        
        // 应用移动 (保持当前Y轴速度，只改变X轴速度)
        rb.velocity = new Vector3(movement.x, rb.velocity.y, rb.velocity.z);
    }
}
