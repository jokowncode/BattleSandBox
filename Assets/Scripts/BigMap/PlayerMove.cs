
using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour{

    [SerializeField] private float FootstepCycle = 4.0f;
    [SerializeField] private float Speed = 5.0f;
    [SerializeField] private Transform RendererTransform;
    
    private Animator PlayerAnimator;
    private BoxCollider PlayerInAreaCollider;
    private float LastPlayFootstepTime = -1.0f;
    
    private void Awake(){
        PlayerAnimator = GetComponentInChildren<Animator>();
    }

    public void SetInAreaCollider(BoxCollider inAreaCollider){
        this.PlayerInAreaCollider = inAreaCollider;
    }
    
    private void Update(){
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 velocity = new Vector3(x, 0.0f, 0.0f);

        Vector3 newPos = this.transform.position + Speed * Time.deltaTime * velocity;
        if (PlayerInAreaCollider && !PlayerInAreaCollider.bounds.Contains(newPos)){
            return;
        }

        this.transform.position = newPos;
        PlayerAnimator.SetFloat(AnimationParams.Velocity, Mathf.Abs(x));
        if (Mathf.Abs(x) != 0){
            if (LastPlayFootstepTime < 0.0f || Time.time - LastPlayFootstepTime >= FootstepCycle){
                LastPlayFootstepTime = Time.time;
                AudioManager.Instance.PlayFootstep();    
            }
        } else{
            LastPlayFootstepTime = -1.0f;
            AudioManager.Instance.StopFootstep();
        }
        // this.transform.position += Speed * Time.deltaTime * velocity;
        
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

