
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FighterMove : MonoBehaviour{

    [field: SerializeField] public Transform RendererTransform{ get; private set; }

    private Fighter Owner;
    private NavMeshAgent Agent;
    private NavMeshObstacle Obstacle;
    
    private void Awake(){
        Owner = GetComponent<Fighter>();
        Agent = GetComponent<NavMeshAgent>();
        Obstacle = GetComponent<NavMeshObstacle>();
        
        Agent.updateRotation = false;
        Agent.speed = Owner.Speed;
        // Obstacle.carveOnlyStationary = true;

        Agent.enabled = true;
        Obstacle.enabled = false;
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
    
    public void MoveTo(Vector3 targetPos){
        // this.Obstacle.enabled = false;
        // this.Obstacle.carving = false;
        this.Agent.enabled = true;
        
        Vector3 velocityDir = (targetPos - this.transform.position).normalized;
        this.Owner.FighterAnimator.SetFloat(AnimationParams.Velocity, velocityDir.sqrMagnitude);
        if (velocityDir == Vector3.zero){
            return;
        }

        //  Control Character Face Forward
        // ChangeForward(velocityDir.x);
        
        //this.transform.position += Owner.Speed * Time.deltaTime * velocityDir;
        Agent.SetDestination(targetPos);
    }

    private void Update(){
        if(this.Agent.enabled) ChangeForward(this.Agent.velocity.x);
    }

    public void StartMove(){
        if (this.Agent.enabled) return;
        this.Obstacle.carving = false;
        this.Obstacle.enabled = false;
    }

    public void StopMove(){
        if (!this.Agent.enabled) return;
        this.Agent.enabled = false;
        this.Obstacle.enabled = true;
        this.Obstacle.carving = true;
    }
}


