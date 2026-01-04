
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour{

    [SerializeField] private float FootstepCycle = 4.0f;
    [SerializeField] private float HorizontalSpeed = 5.0f;
    [SerializeField] private float VerticalSpeed = 2.0f;
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

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 velocity = new Vector3(x, 0.0f, z).normalized;

        if (Physics.BoxCast(this.transform.position + Vector3.up * 0.5f, Vector3.one * 0.05f, velocity, 
                Quaternion.identity, 0.1f, 1 << LayerMask.NameToLayer("Wall"))) {
            PlayerAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
            return;
        }

        velocity.x *= HorizontalSpeed;
        velocity.z *= VerticalSpeed;
        Vector3 newPos = this.transform.position + Time.deltaTime * velocity;
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

        this.transform.position = newPos;
        PlayerAnimator.SetFloat(AnimationParams.Velocity, Vector3.SqrMagnitude(velocity));
        if (Vector3.SqrMagnitude(velocity) != 0){
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

