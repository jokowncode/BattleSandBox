
using System;
using UnityEngine;

public class FighterMove : MonoBehaviour{

    [field: SerializeField] public Transform RendererTransform{ get; private set; }

    private Fighter Owner;

    private void Awake(){
        Owner = GetComponent<Fighter>();
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
    
    public void MoveByDirection(Vector3 velocityDir) {
        this.Owner.FighterAnimator.SetFloat(AnimationParams.Velocity, velocityDir.sqrMagnitude);
        if (velocityDir == Vector3.zero){
            return;
        }

        //  Control Character Face Forward
        ChangeForward(velocityDir.x);
        
        // TODO: Auto Find Way -> NavMeshAgent
        this.transform.position += Owner.Speed * Time.deltaTime * velocityDir;
    }
}


