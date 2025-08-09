using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PositionBinder : MonoBehaviour{
    
    // TODO: IF Enemy Can Bullet Chain too
    private TargetType Type = TargetType.Enemy;
    private FlashPoint[] EndPoints;
    private float Damage;
    private BoxCollider FlashCollider;
    private VisualEffect vfx;
    
    private void Awake(){
        FlashCollider = GetComponent<BoxCollider>();
        vfx = this.GetComponent<VisualEffect>();
    }

    public void SetEndPoints(FlashPoint[] points){
        if(points.Length != 2) throw new System.Exception("Wrong number of points");
        this.EndPoints = points;
        this.Damage = (this.EndPoints[0].Damage + this.EndPoints[1].Damage) / 2.0f;
    }

    private void Update(){
        if (this.EndPoints.Length != 2) return;
        vfx.SetVector3("StartPos", this.EndPoints[0].transform.position);
        vfx.SetVector3("EndPos", this.EndPoints[1].transform.position);
        FlashCollider.center = (this.EndPoints[0].transform.position + this.EndPoints[1].transform.position) / 2.0f;
    }
    
    private void OnTriggerEnter(Collider other){
        if (other.TryGetComponent(out Fighter fighter) && fighter.gameObject.layer == LayerMask.NameToLayer(Type.ToString())) {
            fighter.BeDamaged(new EffectData{
                Value = Damage
            });
        }
    }
}
