
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour{

    [SerializeField] private float FootstepCycle = 4.0f;
    [SerializeField] private float Speed = 5.0f;
    [SerializeField] private Transform RendererTransform;
    
    private Animator PlayerAnimator;
    private BoxCollider PlayerInAreaCollider;
    private PlayerInAreaColliderDir PlayerInAreaColliderDir;
    
    private float LastPlayFootstepTime = -1.0f;

    private void Awake(){
        PlayerAnimator = GetComponentInChildren<Animator>();
    }

    public void SetInAreaCollider(BoxCollider inAreaCollider, PlayerInAreaColliderDir dir) {
        this.PlayerInAreaColliderDir = dir;
        this.PlayerInAreaCollider = inAreaCollider;
    }

    private void OnDisable(){
        PlayerAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
    }

    private void Update() {
        if (DialogManager.Instance.IsInDialog) {
            PlayerAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
            return;
        }
        
        if (!EdgeManager.Instance) {
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        Vector3 velocity = new Vector3(x, 0.0f, 0.0f);

        Vector3 newPos = this.transform.position + Speed * Time.deltaTime * velocity;
        if (PlayerInAreaCollider) {
            bool isStop = false;
            switch (this.PlayerInAreaColliderDir) {
                case PlayerInAreaColliderDir.Both:
                    if (!PlayerInAreaCollider.bounds.Contains(newPos)) {
                        isStop = true;
                    }
                    break;
                case PlayerInAreaColliderDir.Left:
                    if (x > 0.0f && newPos.x + 0.5f > this.PlayerInAreaCollider.bounds.min.x) {
                        isStop = true;
                    }
                    break;
                case PlayerInAreaColliderDir.Right:
                    if (x < 0.0f && newPos.x - 0.5f < this.PlayerInAreaCollider.bounds.max.x) {
                        isStop = true;
                    }
                    break;
            }

            if (isStop) {
                PlayerAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
                return;
            }
        }

        if (newPos.x - 0.5f <= EdgeManager.Instance.LeftEdgeX || newPos.x + 0.5f >= EdgeManager.Instance.RightEdgeX) {
            PlayerAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
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

