
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FighterMove : MonoBehaviour{

    [field: SerializeField] public Transform RendererTransform{ get; private set; }

    private Fighter Owner;
    public NavMeshAgent Agent{ get; private set; }
    private NavMeshObstacle Obstacle;

    private bool CanMove = true;
    private Vector3 CurrentTargetPos;
    private bool IsArrive = false;

    public Action<Fighter> OnArriveDestination;
    
    private void Awake(){
        Owner = GetComponent<Fighter>();
        Agent = GetComponent<NavMeshAgent>();
        Obstacle = GetComponent<NavMeshObstacle>();
        
        Agent.updateRotation = false;
        Agent.speed = Owner.InitialSpeed;
        // Obstacle.carveOnlyStationary = true;

        if (!Agent.enabled) {
            Obstacle.enabled = false;
            Agent.enabled = true;
        }
        this.enabled = false;
    }

    public void ChangeForward(float sign) {
        float scaleX = RendererTransform.localScale.x;
        if (sign > 0.0f) {
            scaleX = Mathf.Abs(scaleX);
        } else if (sign < 0.0f) {
            scaleX = -Mathf.Abs(scaleX);
        }
        RendererTransform.localScale = new Vector3(scaleX, 
            RendererTransform.localScale.y, RendererTransform.localScale.z);
    }

    public void ChangeSpeed(float speed) {
        this.Agent.speed = speed;
        float per = speed / Mathf.Max(Owner.InitialSpeed, 0.1f);
        Owner.FighterAnimator.SetFloat(AnimationParams.WalkAnimSpeedMultiplier, per);
    }

    public void MoveTo(Vector3 targetPos) {
        if (!CanMove) return;
        if (this.Owner.IsDead) return;
        
        this.Agent.enabled = true;
        this.Agent.SetDestination(targetPos);
        this.CurrentTargetPos = targetPos;
        this.CurrentTargetPos.y = 0.0f;
        
        Vector3 velocityDir = (targetPos - this.transform.position).normalized;
        this.Owner.FighterAnimator.SetFloat(AnimationParams.Velocity, velocityDir.sqrMagnitude);
        if (velocityDir == Vector3.zero){
            IsArrive = true;
            OnArriveDestination?.Invoke(this.Owner);
            this.enabled = false;
            return;
        }
        IsArrive = false;
        this.enabled = true;
    }
    
    private Vector3 GenerateRandomPoint(Vector3 center, float radius) {
        Vector2 randomDir = UnityEngine.Random.insideUnitCircle * radius;
        Vector3 targetPos = center + new Vector3(randomDir.x, 0, randomDir.y);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas)) {
            return hit.position;
        }
        return center;
    }

    private void Update(){
        if (this.Agent.enabled) {
            ChangeForward(this.Agent.velocity.x);
            Vector3 currentPos = this.transform.position;
            currentPos.y = 0.0f;
            if (!IsArrive && (CurrentTargetPos - currentPos).sqrMagnitude <= 0.005f) {
                IsArrive = true;
                this.enabled = false;
                OnArriveDestination?.Invoke(this.Owner);
            }
        }
    }

    public void StartMove(){
        if (this.Agent.enabled) return;
        this.Obstacle.carving = false;
        this.Obstacle.enabled = false;
        this.CanMove = true;
    }

    public void StopMove(){
        if (!this.Agent.enabled) return;
        this.Agent.enabled = false;
        this.Obstacle.enabled = true;
        this.Obstacle.carving = true;
        this.CanMove = false;
    }
}


