
using System;
using UnityEngine;

public class Move : MonoBehaviour{

    [SerializeField] private float Speed = 5.0f;
    [SerializeField] private Transform RendererTransform;

    private Animator PlayerAnimator;

    private void Awake(){
        PlayerAnimator = GetComponentInChildren<Animator>();
    }
    
    private void Update(){
        float x = Input.GetAxisRaw("Horizontal");
        PlayerAnimator.SetFloat(AnimationParams.Velocity, Mathf.Abs(x));
        Vector3 velocity = new Vector3(x, 0.0f, 0.0f);
        this.transform.position += Speed * Time.deltaTime * velocity;
        
        float scaleX = RendererTransform.localScale.x;
        if (x > 0.0f) {
            scaleX = Mathf.Abs(scaleX);
        } else if (x < 0.0f) {
            scaleX = -Mathf.Abs(scaleX);
        }
        
        RendererTransform.localScale = new Vector3(scaleX, 
            RendererTransform.localScale.y, RendererTransform.localScale.z);
    }
}

